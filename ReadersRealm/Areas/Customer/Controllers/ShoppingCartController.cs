﻿namespace ReadersRealm.Web.Areas.Customer.Controllers;

using Data.Models;
using DataTransferObjects.OrderDetails;
using DataTransferObjects.OrderHeader;
using Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using ReadersRealm.Areas;
using ReadersRealm.ViewModels.ApplicationUser;
using ReadersRealm.ViewModels.OrderDetails;
using ReadersRealm.ViewModels.OrderHeader;
using ReadersRealm.ViewModels.ShoppingCart;
using Services.Contracts;
using Stripe.Checkout;
using System.Text;
using static Common.Constants.Constants.Areas;
using static Common.Constants.Constants.OrderHeader;
using static Common.Constants.Constants.Roles;
using static Common.Constants.Constants.SessionKeys;
using static Common.Constants.Constants.Shared;
using static Common.Constants.Constants.ShoppingCart;
using static Common.Constants.Constants.StripeSettings;

[Area(Customer)]
public class ShoppingCartController : BaseController
{
    private readonly IShoppingCartService _shoppingCartService;
    private readonly IApplicationUserService _applicationUserService;
    private readonly IOrderService _orderService;
    private readonly IOrderHeaderService _orderHeaderService;
    private readonly IOrderDetailsService _orderDetailsService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ShoppingCartController(
        IShoppingCartService shoppingCartService, 
        IApplicationUserService applicationUserService,
        IOrderService orderService,
        IOrderHeaderService orderHeaderService,
        IOrderDetailsService orderDetailsService,
        IHttpContextAccessor httpContextAccessor)
    {
        _shoppingCartService = shoppingCartService;
        _applicationUserService = applicationUserService;
        _orderService = orderService;
        _orderHeaderService = orderHeaderService;
        _orderDetailsService = orderDetailsService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        string userId = User.GetId();

        AllShoppingCartsListViewModel shoppingCartModel = await _shoppingCartService
            .GetAllListAsync(userId);

        return View(shoppingCartModel);
    }

    [HttpGet]
    public async Task<IActionResult> Summary()
    {
        string userId = User.GetId();

        AllShoppingCartsListViewModel shoppingCartModel = await _shoppingCartService
            .GetAllListAsync(userId);

        return View(shoppingCartModel);
    }

    [HttpPost]
    public async Task<IActionResult> Summary(AllShoppingCartsListViewModel shoppingCartModel)
    {
        string userId = User.GetId();
        if (!ModelState.IsValid)
        {
            shoppingCartModel = await _shoppingCartService
                .GetAllListAsync(userId);

            return View(shoppingCartModel);
        }

        OrderHeaderViewModel? orderHeaderModel = await _orderHeaderService
            .GetByApplicationUserIdAndOrderStatusAsync(shoppingCartModel.OrderHeader.ApplicationUserId, OrderStatusPending);

        if (orderHeaderModel != null)
        {
            await _orderDetailsService
                .DeleteOrderDetailsRangeByOrderHeaderIdAsync(orderHeaderModel.Id);

            await _orderHeaderService
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

        await _applicationUserService
            .UpdateApplicationUserAsync(applicationUserModel);

        shoppingCartModel = await _shoppingCartService
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
        (Guid orderHeaderId, Guid orderId) orderHeaderData = await _orderHeaderService
            .CreateOrderHeaderAsync(shoppingCartModel.OrderHeader);

        foreach (ShoppingCartViewModel shoppingCart in shoppingCartModel.ShoppingCartsList)
        {
            OrderDetailsViewModel orderDetailsModel = new OrderDetailsViewModel()
            {
                BookId = shoppingCart.BookId,
                OrderHeaderId = orderHeaderData.orderHeaderId,
                Count = shoppingCart.Count,
                Price = shoppingCart.TotalPrice,
                Order = await _orderService.GetOrderForSummaryAsync(orderHeaderData.orderId)
            };

            await _orderDetailsService
                .CreateOrderDetailsAsync(orderDetailsModel);
        }

        if (!isUserInCompanyRole)
        {
            Session session = await ConfigureStripe(orderHeaderData.orderHeaderId, shoppingCartModel);

            await _orderHeaderService
                .UpdateOrderHeaderPaymentIntentIdAsync(orderHeaderData.orderHeaderId, session.Id, session.PaymentIntentId);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        return RedirectToAction(nameof(OrderConfirmation), nameof(ShoppingCart), new { orderHeaderId = orderHeaderData.orderHeaderId }); 
    }

    [HttpGet]
    public async Task<IActionResult> OrderConfirmation(Guid orderHeaderId)
    {
        OrderHeaderViewModel orderHeaderModel = await _orderHeaderService
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

                await _orderHeaderService
                    .UpdateOrderHeaderAsync(orderHeaderModel);
            }
        }

        string applicationUserId = User.GetId();
        await _shoppingCartService
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

        OrderHeaderReceiptDto orderHeaderDto = await _orderHeaderService
            .GetOrderHeaderForReceiptAsync((Guid)orderHeaderId);

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

        _httpContextAccessor.HttpContext.Response.Headers.Add(HeaderNames.ContentDisposition, "attachment;filename=receipt.txt");

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

        await _shoppingCartService
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

        bool isItemDeleted = await _shoppingCartService
            .DecreaseQuantityForShoppingCartAsync((Guid)id);

        if (isItemDeleted)
        {
            await SetShoppingCartItemsCountInSession();
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _shoppingCartService
            .DeleteShoppingCartAsync(id);

        await SetShoppingCartItemsCountInSession();

        TempData[Success] = ShoppingCartItemHasBeenDeletedSuccessfully;

        return RedirectToAction(nameof(Index));
    }

    private async Task<Session> ConfigureStripe(Guid orderHeaderId, AllShoppingCartsListViewModel shoppingCartModel)
    {
        string domain = _httpContextAccessor.GetDomain();
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
        string userId = User.GetId();

        int itemsCount = await _shoppingCartService
            .GetShoppingCartCountByApplicationUserIdAsync(userId);

        HttpContext.Session.SetInt32(ShoppingCartSessionKey, itemsCount);
    }
}