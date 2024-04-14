namespace ReadersRealm.Web.Areas.Customer.Controllers;

using System.Text;
using Data.Models;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Services.Data.ApplicationUserServices.Contracts;
using Services.Data.Models.OrderDetails;
using Services.Data.Models.OrderHeader;
using Services.Data.OrderDetailsServices.Contracts;
using Services.Data.OrderHeaderServices.Contracts;
using Services.Data.OrderServices.Contracts;
using Services.Data.ShoppingCartServices.Contracts;
using Stripe.Checkout;
using ViewModels.ApplicationUser;
using ViewModels.OrderDetails;
using ViewModels.OrderHeader;
using ViewModels.ShoppingCart;
using static Common.Constants.Constants.AreasConstants;
using static Common.Constants.Constants.ErrorConstants;
using static Common.Constants.Constants.OrderHeaderConstants;
using static Common.Constants.Constants.RolesConstants;
using static Common.Constants.Constants.SendGridSettingsConstants;
using static Common.Constants.Constants.SessionKeysConstants;
using static Common.Constants.Constants.SharedConstants;
using static Common.Constants.Constants.ShoppingCartConstants;
using static Common.Constants.Constants.StripeSettingsConstants;

[Area(Customer)]
public class ShoppingCartController(
    IApplicationUserCrudService applicationUserCrudService,
    IHttpContextAccessor httpContextAccessor,
    IEmailSender emailSender,
    IShoppingCartCrudService shoppingCartCrudService,
    IShoppingCartRetrievalService shoppingCartRetrievalService,
    IShoppingCartModificationService shoppingCartModificationService,
    IOrderHeaderCrudService orderHeaderCrudService,
    IOrderHeaderRetrievalService orderHeaderRetrievalService,
    IOrderDetailsCrudService orderDetailsCrudService,
    IOrderRetrievalService orderRetrievalService)
    : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        Guid userId = User.GetId();

        AllShoppingCartsListViewModel shoppingCartModel = await shoppingCartRetrievalService
            .GetAllListAsync(userId);

        return View(shoppingCartModel);
    }

    [HttpGet]
    public async Task<IActionResult> Summary()
    {
        Guid userId = User.GetId();

        AllShoppingCartsListViewModel shoppingCartModel = await shoppingCartRetrievalService
            .GetAllListAsync(userId);

        return View(shoppingCartModel);
    }

    [HttpPost]
    public async Task<IActionResult> Summary(AllShoppingCartsListViewModel shoppingCartModel)
    {
        Guid userId = User.GetId();

        if (!ModelState.IsValid)
        {
            shoppingCartModel = await shoppingCartRetrievalService
                .GetAllListAsync(userId);

            return View(shoppingCartModel);
        }

        OrderHeaderViewModel? orderHeaderModel = await orderHeaderRetrievalService
            .GetByApplicationUserIdAndOrderStatusAsync(shoppingCartModel.OrderHeader.ApplicationUserId, OrderStatusPending);

        if (orderHeaderModel != null)
        {
            await orderDetailsCrudService
                .DeleteOrderDetailsRangeByOrderHeaderIdAsync(orderHeaderModel.Id);

            await orderHeaderCrudService
                .DeleteOrderHeaderAsync(orderHeaderModel);
        }

        OrderApplicationUserViewModel applicationUserModel = new OrderApplicationUserViewModel()
        {
            Id = shoppingCartModel.OrderHeader.ApplicationUserId,
            FirstName = shoppingCartModel.OrderHeader.FirstName,
            LastName = shoppingCartModel.OrderHeader.LastName,
            StreetAddress = shoppingCartModel.OrderHeader.StreetAddress,
            City = shoppingCartModel.OrderHeader.City,
            State = shoppingCartModel.OrderHeader.State,
            PostalCode = shoppingCartModel.OrderHeader.PostalCode,
            PhoneNumber = shoppingCartModel.OrderHeader.PhoneNumber,
        };

        await applicationUserCrudService
            .UpdateApplicationUserAsync(applicationUserModel);

        shoppingCartModel = await shoppingCartRetrievalService
            .GetAllListAsync(userId);

        bool isUserInCompanyRole = User.IsInRole(CompanyRole);
        if (isUserInCompanyRole)
        {
            //Order Confirmation -> Processing -> Shipping -> Make Payment
            shoppingCartModel.OrderHeader.PaymentStatus = PaymentStatusDelayedPayment;
            shoppingCartModel.OrderHeader.OrderStatus = OrderStatusApproved;
        }
        else
        {
            //Make Payment -> Order Confirmation -> Processing -> Shipping
            shoppingCartModel.OrderHeader.PaymentStatus = PaymentStatusPending;
            shoppingCartModel.OrderHeader.OrderStatus = OrderStatusPending;
        }

        shoppingCartModel.OrderHeader.OrderDate = DateTime.Now;
        (Guid orderHeaderId, Guid orderId) orderHeaderData = await orderHeaderCrudService
            .CreateOrderHeaderAsync(shoppingCartModel.OrderHeader);

        foreach (ShoppingCartViewModel shoppingCart in shoppingCartModel.ShoppingCartsList)
        {
            OrderDetailsViewModel orderDetailsModel = new OrderDetailsViewModel()
            {
                BookId = shoppingCart.BookId,
                OrderHeaderId = orderHeaderData.orderHeaderId,
                Count = shoppingCart.Count,
                Price = shoppingCart.TotalPrice,
                Order = await orderRetrievalService
                    .GetOrderForSummaryAsync(orderHeaderData.orderId)
            };

            await orderDetailsCrudService
                .CreateOrderDetailsAsync(orderDetailsModel);
        }

        if (!isUserInCompanyRole)
        {
            Session session = await ConfigureStripe(orderHeaderData.orderHeaderId, shoppingCartModel);

            await orderHeaderCrudService
                .UpdateOrderHeaderPaymentIntentIdAsync(
                    orderHeaderData.orderHeaderId, 
                    session.Id, 
                    session.PaymentIntentId);

            Response.Headers.Append("Location", session.Url);
            return new StatusCodeResult(303);
        }

        return RedirectToAction(nameof(OrderConfirmation), nameof(ShoppingCart), new { id = orderHeaderData.orderHeaderId }); 
    }

    [HttpGet]
    public async Task<IActionResult> OrderConfirmation(Guid? id)
    {
        if (id is not { } orderHeaderId || id == Guid.Empty)
        {
            return RedirectToAction(ErrorPageNotFoundAction, nameof(Error));
        }

        OrderHeaderViewModel orderHeaderModel = await orderHeaderRetrievalService
            .GetByIdAsyncWithNavPropertiesAsync(orderHeaderId);

        bool isUserInCompanyRole = User.IsInRole(CompanyRole);
        if (!isUserInCompanyRole)
        {
            SessionService service = new SessionService();
            Session session = await service.GetAsync(orderHeaderModel.SessionId);

            if (session.PaymentStatus.ToLower() == "paid")
            {
                orderHeaderModel.PaymentDate = DateTime.Now;
                orderHeaderModel.SessionId = session.Id;
                orderHeaderModel.PaymentIntentId = session.PaymentIntentId;
                orderHeaderModel.OrderStatus = OrderStatusApproved;
                orderHeaderModel.PaymentStatus = PaymentStatusApproved;

                await orderHeaderCrudService
                    .UpdateOrderHeaderAsync(orderHeaderModel);

                await emailSender
                    .SendEmailAsync(orderHeaderModel.ApplicationUser.Email, 
                        EmailOrderSubject, 
                        this.BuildEmailMessage(orderHeaderModel.ApplicationUser.FirstName, orderHeaderId.ToString()));
            }
        }

        Guid applicationUserId = User.GetId();
        await shoppingCartCrudService
            .DeleteAllShoppingCartsApplicationUserIdAsync(applicationUserId);

        return View(orderHeaderId);
    }

    [HttpGet]
    public async Task<IActionResult> DownloadReceipt(Guid? id)
    {
        if (id is not { } orderHeaderId || id == Guid.Empty)
        {
            return RedirectToAction(ErrorPageNotFoundAction, nameof(Error));
        }
    
        OrderHeaderReceiptDto orderHeaderDto = await orderHeaderRetrievalService
            .GetOrderHeaderForReceiptAsync(orderHeaderId);
    
        StringBuilder sb = new StringBuilder();
    
        sb.AppendLine($"Order Id: {orderHeaderId}");
        sb.AppendLine($"Order Payment Id: {orderHeaderDto.PaymentIntentId}");
        sb.AppendLine($"Order Date: {orderHeaderDto.OrderDate.ToString("d")}");
        sb.AppendLine($"Total: {orderHeaderDto.OrderTotal.ToString("c")}");
        sb.AppendLine($"Payment Status: {orderHeaderDto.PaymentStatus}");
        sb.AppendLine($"Payment Date: {orderHeaderDto.PaymentDate}");
        sb.AppendLine("Items Bought:");
    
        foreach (OrderDetailsReceiptDto orderDetailsDto in orderHeaderDto.OrderDetails)
        {
            sb.AppendLine("--------------------------------------------");
            sb.AppendLine($"    Book Title: {orderDetailsDto.Book.Title}");
            sb.AppendLine($"    Book ISBN: {orderDetailsDto.Book.ISBN}");
            sb.AppendLine($"    Book Price: {orderDetailsDto.Book.Price.ToString("c")}");
            sb.AppendLine($"    Quantity: {orderDetailsDto.Count}");
            sb.AppendLine($"    Total Price: {(orderDetailsDto.Book.Price * orderDetailsDto.Count).ToString("c")}");
        }
    
        httpContextAccessor.HttpContext!.Response.Headers.Append(HeaderNames.ContentDisposition, "attachment;filename=receipt.txt");
    
        byte[] textBytes = Encoding.UTF8.GetBytes(sb.ToString().TrimEnd());
    
        return File(textBytes, "text/plain");
    }

    [HttpPost]
    public async Task<IActionResult> IncreaseQuantity(Guid? id)
    {
        if (id is not { } shoppingCartId || id == Guid.Empty)
        {
            return RedirectToAction(ErrorPageNotFoundAction, nameof(Error));
        }

        await shoppingCartModificationService
            .IncreaseShoppingCartQuantityAsync(shoppingCartId);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> DecreaseQuantity(Guid? id)
    {
        if (id is not { } shoppingCartId || id == Guid.Empty)
        {
            return RedirectToAction(ErrorPageNotFoundAction, nameof(Error));
        }

        bool isItemDeleted = await shoppingCartModificationService
            .DecreaseShoppingCartQuantityAsync(shoppingCartId);

        if (isItemDeleted)
        {
            await SetShoppingCartItemsCountInSession();
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id is not { } shoppingCartId || id == Guid.Empty)
        {
            return RedirectToAction(ErrorPageNotFoundAction, nameof(Error));
        }

        await shoppingCartCrudService
            .DeleteShoppingCartAsync(shoppingCartId);

        await SetShoppingCartItemsCountInSession();

        TempData[Success] = ShoppingCartItemHasBeenDeletedSuccessfully;

        return RedirectToAction(nameof(Index));
    }

    private async Task<Session> ConfigureStripe(Guid orderHeaderId, AllShoppingCartsListViewModel shoppingCartModel)
    {
        string domain = httpContextAccessor.GetDomain();
        SessionCreateOptions options = new SessionCreateOptions
        {
            LineItems = new List<SessionLineItemOptions>(),
            SuccessUrl = string.Format(FullSuccessUrlShoppingCart, domain, orderHeaderId),
            CancelUrl = string.Format(FullCancelUrlShoppingCart, domain),
            Mode = "payment",
        };

        foreach (ShoppingCartViewModel shoppingCart in shoppingCartModel.ShoppingCartsList)
        {
            options.LineItems.Add(new SessionLineItemOptions()
            {
                PriceData = new SessionLineItemPriceDataOptions()
                {
                    UnitAmountDecimal = shoppingCart.Count <= 50 ? Math.Round(shoppingCart.Book.Price * 100) :
                        shoppingCart.Count is > 50 and <= 100 ?
                            Math.Round((shoppingCart.Book.Price * 100) - ((shoppingCart.Book.Price * 100) * 0.1M)) :
                            Math.Round((shoppingCart.Book.Price * 100) - ((shoppingCart.Book.Price * 100) * 0.2M)),
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions()
                    {
                        Name = shoppingCart.Book.Title,
                    }
                },
                Quantity = shoppingCart.Count,
            });
        }

        SessionService service = new SessionService();
        return await service.CreateAsync(options);
    }

    private async Task SetShoppingCartItemsCountInSession()
    {
        Guid userId = User.GetId();

        int itemsCount = await shoppingCartRetrievalService
            .GetShoppingCartCountByApplicationUserIdAsync(userId);

        HttpContext.Session.SetInt32(ShoppingCartSessionKey, itemsCount);
    }

    private string BuildEmailMessage(string userFirstName, string orderId)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine(string.Format(EmailOrderHeaderMessage, userFirstName));
        sb.AppendLine(string.Format(EmailOrderBodyMessage, orderId));
        sb.AppendLine(EmailOrderFooterMessage);

        return sb.ToString().TrimEnd();
    }
}