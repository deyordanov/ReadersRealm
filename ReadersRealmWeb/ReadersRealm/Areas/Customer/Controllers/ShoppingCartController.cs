namespace ReadersRealm.Areas.Customer.Controllers;

using Data.Models;
using Extensions.ClaimsPrincipal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using ReadersRealm.Extensions.HttpContextAccessor;
using Services.Contracts;
using Stripe.Checkout;
using System.Text;
using ViewModels.ApplicationUser;
using ViewModels.OrderHeader;
using ViewModels.ShoppingCart;
using Web.ViewModels.OrderDetails;
using static Common.Constants.Constants.Areas;
using static Common.Constants.Constants.OrderHeader;
using static Common.Constants.Constants.Roles;
using static Common.Constants.Constants.Shared;
using static Common.Constants.Constants.ShoppingCart;
using static Common.Constants.Constants.StripeSettings;

[Area(Customer)]
public class ShoppingCartController : BaseController
{
    private readonly IShoppingCartService _shoppingCartService;
    private readonly IApplicationUserService _applicationUserService;
    private readonly IOrderHeaderService _orderHeaderService;
    private readonly IOrderDetailsService _orderDetailsService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ShoppingCartController(
        IShoppingCartService shoppingCartService, 
        IApplicationUserService applicationUserService,
        IOrderHeaderService orderHeaderService,
        IOrderDetailsService orderDetailsService,
        IHttpContextAccessor httpContextAccessor)
    {
        this._shoppingCartService = shoppingCartService;
        this._applicationUserService = applicationUserService;
        this._orderHeaderService = orderHeaderService;
        this._orderDetailsService = orderDetailsService;
        this._httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        string userId = User.GetId();

        AllShoppingCartsListViewModel shoppingCartModel = await this
            ._shoppingCartService
            .GetAllListAsync(userId);

        return View(shoppingCartModel);
    }

    [HttpGet]
    public async Task<IActionResult> Summary()
    {
        string userId = User.GetId();

        AllShoppingCartsListViewModel shoppingCartModel = await this
            ._shoppingCartService
            .GetAllListAsync(userId);

        return View(shoppingCartModel);
    }

    [HttpPost]
    public async Task<IActionResult> Summary(AllShoppingCartsListViewModel shoppingCartModel)
    {
        string userId = User.GetId();
        if (!ModelState.IsValid)
        {
            shoppingCartModel = await this
                ._shoppingCartService
                .GetAllListAsync(userId);

            return View(shoppingCartModel);
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

        await this
            ._applicationUserService
            .UpdateApplicationUserAsync(applicationUserModel);

        shoppingCartModel = await this
            ._shoppingCartService
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
        Guid orderHeaderId = await this
            ._orderHeaderService
            .CreateOrderHeaderAsync(shoppingCartModel.OrderHeader);

        foreach (ShoppingCartViewModel shoppingCart in shoppingCartModel.ShoppingCartsList)
        {
            OrderDetailsViewModel orderDetailsModel = new OrderDetailsViewModel()
            {
                BookId = shoppingCart.BookId,
                OrderHeaderId = orderHeaderId,
                Count = shoppingCart.Count,
                Price = shoppingCart.TotalPrice
            };

            await this
                ._orderDetailsService
                .CreateOrderDetailsAsync(orderDetailsModel);
        }

        if (!isUserInCompanyRole)
        {
            Session session = await this.ConfigureStripe(orderHeaderId, shoppingCartModel);

            await this
                ._orderHeaderService
                .UpdateOrderHeaderPaymentIntentIdAsync(orderHeaderId, session.Id, session.PaymentIntentId);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        return RedirectToAction(nameof(OrderConfirmation), nameof(ShoppingCart), new { orderHeaderId = orderHeaderId }); 
    }

    [HttpGet]
    public async Task<IActionResult> OrderConfirmation(Guid orderHeaderId)
    {
        OrderHeaderViewModel orderHeaderModel = await this
            ._orderHeaderService
            .GetByIdAsyncWithNavPropertiesAsync(orderHeaderId);

        bool isUserInCompanyRole = this.User.IsInRole(CompanyRole);
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

                await this
                    ._orderHeaderService
                    .UpdateOrderHeaderAsync(orderHeaderModel);

                // await this
                //     ._orderHeaderService
                //     .UpdateOrderHeaderPaymentIntentIdAsync(orderHeaderModel.Id, session.Id, session.PaymentIntentId);
                //
                // await this
                //     ._orderHeaderService
                //     .UpdateOrderHeaderStatusAsync(orderHeaderModel.Id, OrderStatusApproved, PaymentStatusApproved);
            }
        }

        string applicationUserId = this.User.GetId();
        await this
            ._shoppingCartService
            .DeleteAllShoppingCartsApplicationUserIdAsync(applicationUserId);

        return View(orderHeaderId);
    }

    [HttpGet]
    public async Task<IActionResult> DownloadReceipt(Guid? orderHeaderId)
    {
        if (orderHeaderId == null || orderHeaderId == Guid.Empty)
        {
            return NotFound();
        }

        OrderHeaderReceiptViewModel orderHeaderModel = await this
            ._orderHeaderService
            .GetOrderHeaderForReceiptAsync((Guid)orderHeaderId);

        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"Order Id: {orderHeaderId}");
        sb.AppendLine($"Order Payment Id: {orderHeaderModel.PaymentIntentId}");
        sb.AppendLine($"Order Date: {orderHeaderModel.OrderDate.ToString("d")}");
        sb.AppendLine($"Total: {orderHeaderModel.OrderTotal.ToString("c")}");
        sb.AppendLine($"Payment Status: {orderHeaderModel.PaymentStatus}");
        sb.AppendLine($"Payment Date: {orderHeaderModel.PaymentDate}");
        sb.AppendLine("Items Bought:");

        foreach (OrderDetailsViewModel orderDetailsModel in orderHeaderModel.OrderDetails)
        {
            sb.AppendLine("--------------------------------------------");
            sb.AppendLine($"    Book Title: {orderDetailsModel.Book.Title}");
            sb.AppendLine($"    Book ISBN: {orderDetailsModel.Book.ISBN}");
            sb.AppendLine($"    Book Price: {orderDetailsModel.Book.Price.ToString("c")}");
            sb.AppendLine($"    Quantity: {orderDetailsModel.Count}");
            sb.AppendLine($"    Total Price: {(orderDetailsModel.Book.Price * orderDetailsModel.Count).ToString("c")}");
            sb.AppendLine("--------------------------------------------");
        }

        this._httpContextAccessor.HttpContext.Response.Headers.Add(HeaderNames.ContentDisposition, "attachment;filename=receipt.txt");

        byte[] textBytes = Encoding.UTF8.GetBytes(sb.ToString().TrimEnd());

        return File(textBytes, "text/plain");
    }

    [HttpGet]
    public async Task<IActionResult> IncreaseQuantity(Guid? id)
    {
        if (id == null || id == Guid.Empty)
        {
            return NotFound();
        }

        await this
            ._shoppingCartService
            .IncreaseQuantityForShoppingCartAsync((Guid)id);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> DecreaseQuantity(Guid? id)
    {
        if (id == null || id == Guid.Empty)
        {
            return NotFound();
        }

        await this
            ._shoppingCartService
            .DecreaseQuantityForShoppingCartAsync((Guid)id);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        await this
            ._shoppingCartService
            .DeleteShoppingCartAsync(id);

        TempData[Success] = ShoppingCartItemHasBeenDeletedSuccessfully;

        return RedirectToAction(nameof(Index));
    }

    private async Task<Session> ConfigureStripe(Guid orderHeaderId, AllShoppingCartsListViewModel shoppingCartModel)
    {
        string domain = this._httpContextAccessor.GetDomain();
        SessionCreateOptions options = new SessionCreateOptions
        {
            LineItems = new List<SessionLineItemOptions>(),
            SuccessUrl = string.Format(FullSuccessUrl, domain, orderHeaderId),
            CancelUrl = string.Format(FullCancelUrl, domain),
            Mode = "payment",
        };

        foreach (ShoppingCartViewModel shoppingCart in shoppingCartModel.ShoppingCartsList)
        {
            options.LineItems.Add(new SessionLineItemOptions()
            {
                PriceData = new SessionLineItemPriceDataOptions()
                {
                    UnitAmountDecimal = shoppingCart.Book.Price * 100,
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
}