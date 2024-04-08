namespace ReadersRealm.Web.Areas.Admin.Controllers;

using Common;
using Data.Models;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Data.OrderDetailsServices.Contracts;
using Services.Data.OrderHeaderServices.Contracts;
using Services.Data.OrderServices.Contracts;
using Stripe;
using Stripe.Checkout;
using ViewModels.Order;
using ViewModels.OrderDetails;
using ViewModels.OrderHeader;
using static Common.Constants.Constants.AreasConstants;
using static Common.Constants.Constants.OrderConstants;
using static Common.Constants.Constants.OrderHeaderConstants;
using static Common.Constants.Constants.RolesConstants;
using static Common.Constants.Constants.SharedConstants;
using static Common.Constants.Constants.ErrorConstants;
using static Common.Constants.Constants.StripeSettingsConstants;

[Area(Admin)]
public class OrderController(
    IHttpContextAccessor httpContextAccessor,
    IOrderRetrievalService orderRetrievalService,
    IOrderCrudService orderCrudService,
    IOrderHeaderRetrievalService orderHeaderRetrievalService,
    IOrderHeaderCrudService orderHeaderCrudService,
    IOrderDetailsRetrievalService orderDetailsRetrievalService)
    : BaseController
{
    private const string AuthorizeAdminAndEmployeeRoles = $"{AdminRole}, {EmployeeRole}";

    [HttpGet]
    public async Task<IActionResult> Index(int pageIndex, string? searchTerm)
    {
        PaginatedList<AllOrdersViewModel> allOrders;

        bool isUserAdminOrEmployee = User.IsInRole(AdminRole) || User.IsInRole(EmployeeRole);
        if (isUserAdminOrEmployee)
        {
            allOrders = await orderRetrievalService
                .GetAllAsync(pageIndex, 5, searchTerm);
        }
        else
        {
            Guid userId = User.GetId();
            allOrders = await orderRetrievalService
                .GetAllByUserIdAsync(pageIndex, 5, searchTerm, userId);
        }

        ViewBag.PrevDisabled = !allOrders.HasPreviousPage;
        ViewBag.NextDisabled = !allOrders.HasNextPage;
        ViewBag.ControllerName = nameof(Order);
        ViewBag.ActionName = nameof(Index);

        return View(allOrders);
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid? id, int pageIndex, string? searchTerm)
    {
        if (id is not { } orderId || id == Guid.Empty)
        {
            return RedirectToAction(ErrorPageNotFoundAction, nameof(Error));
        }

        DetailsOrderViewModel orderModel = await orderRetrievalService
            .GetOrderForDetailsAsync(orderId);

        ViewBag.PageIndex = pageIndex;
        ViewBag.SearchTerm = searchTerm ?? string.Empty;

        return View(orderModel);
    }

    [HttpPost]
    [Authorize(Roles = AuthorizeAdminAndEmployeeRoles)]
    public async Task<IActionResult> Details(DetailsOrderViewModel orderModel, int pageIndex, string? searchTerm)
    {
        if (!ModelState.IsValid)
        {
            orderModel = await orderRetrievalService
                .GetOrderForDetailsAsync(orderModel.Id);

            return View(orderModel);
        }

        await orderCrudService
            .UpdateOrderAsync(orderModel);

        TempData[Success] = OrderHasSuccessfullyBeenUpdated;

        orderModel = await orderRetrievalService
            .GetOrderForDetailsAsync((Guid)orderModel.Id);

        return View(orderModel);
    }

    [HttpPost]
    [Authorize(Roles = AuthorizeAdminAndEmployeeRoles)]
    public async Task<IActionResult> StartProcessingOrder(Guid? id, int pageIndex, string? searchTerm)
    {
        if (id is not { } orderHeaderId || id == Guid.Empty)
        {
            return RedirectToAction(ErrorPageNotFoundAction, nameof(Error));
        }

        await orderHeaderCrudService
            .UpdateOrderHeaderStatusAsync(orderHeaderId, OrderStatusInProcess, null);

        TempData[Success] = OrderStatusHasSuccessfullyBeenUpdated;

        Guid orderId = await orderRetrievalService
            .GetOrderIdByOrderHeaderIdAsync(orderHeaderId);

        return RedirectToAction(nameof(Details), nameof(Order), new { id = orderId, pageIndex, searchTerm });
    }

    [HttpPost]
    [Authorize(Roles = AuthorizeAdminAndEmployeeRoles)]
    public async Task<IActionResult> ShipOrder(DetailsOrderViewModel orderModel, int pageIndex, string? searchTerm)
    {
        Guid orderId = await orderRetrievalService
            .GetOrderIdByOrderHeaderIdAsync(orderModel.OrderHeaderId);

        if (orderModel.OrderHeader.TrackingNumber == null ||
            orderModel.OrderHeader.Carrier == null)
        {
            TempData[Error] = OrderTrackingNumberAndCarrierAreNotSet;
            return RedirectToAction(nameof(Details), nameof(Order), new { id = orderId, pageIndex, searchTerm });
        }

        orderModel.OrderHeader.OrderStatus = OrderStatusShipped;
        orderModel.OrderHeader.ShippingDate = DateTime.UtcNow;

        if (orderModel.OrderHeader.PaymentStatus == PaymentStatusDelayedPayment)
        {
            orderModel.OrderHeader.PaymentDueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30));
        }

        await orderHeaderCrudService
            .UpdateOrderHeaderAsync(orderModel.OrderHeader);

        TempData[Success] = OrderStatusHasSuccessfullyBeenUpdated;

        return RedirectToAction(nameof(Details), nameof(Order), new { id = orderId, pageIndex, searchTerm });
    }

    [HttpPost]
    [Authorize(Roles = AuthorizeAdminAndEmployeeRoles)]
    public async Task<IActionResult> CancelOrder(DetailsOrderViewModel orderModel, int pageIndex, string? searchTerm)
    {
        if (orderModel.OrderHeader.PaymentStatus == PaymentStatusApproved)
        {
            await InitiateStripeRefund(orderModel);

            await orderHeaderCrudService
                .UpdateOrderHeaderStatusAsync(orderModel.OrderHeader.Id, OrderStatusCancelled, PaymentStatusRefunded);
        }
        else
        {
            await orderHeaderCrudService
                .UpdateOrderHeaderStatusAsync(orderModel.OrderHeader.Id, OrderStatusCancelled, PaymentStatusCancelled);
        }

        TempData[Success] = OrderStatusHasSuccessfullyBeenUpdated;

        Guid orderId = await orderRetrievalService
            .GetOrderIdByOrderHeaderIdAsync(orderModel.OrderHeader.Id);

        return RedirectToAction(nameof(Details), nameof(Order), new { id = orderId, pageIndex, searchTerm });
    }

    [HttpPost]
    public async Task<IActionResult> PayForOrder(DetailsOrderViewModel orderModel, int pageIndex, string? searchTerm)
    {
        orderModel.OrderDetailsList = await orderDetailsRetrievalService
            .GetAllByOrderHeaderIdAsync(orderModel.OrderHeader.Id);

        Session session = await ConfigureStripe(orderModel);

        await orderHeaderCrudService
            .UpdateOrderHeaderPaymentIntentIdAsync(orderModel.OrderHeader.Id, session.Id, session.PaymentIntentId);

        Response.Headers.Add("Location", session.Url);
        return new StatusCodeResult(303);
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

        SessionService service = new SessionService();
        Session session = await service.GetAsync(orderHeaderModel.SessionId);

        if (session.PaymentStatus.ToLower() == "paid")
        {
            orderHeaderModel.PaymentDate = DateTime.Now;
            orderHeaderModel.SessionId = session.Id;
            orderHeaderModel.PaymentIntentId = session.PaymentIntentId;
            orderHeaderModel.OrderStatus = orderHeaderModel.OrderStatus;
            orderHeaderModel.PaymentStatus = PaymentStatusApproved;

            await orderHeaderCrudService
                .UpdateOrderHeaderAsync(orderHeaderModel);
        }

        return View(orderHeaderId);
    }

    private async Task InitiateStripeRefund(DetailsOrderViewModel orderModel)
    {
        RefundCreateOptions refundOptions = new RefundCreateOptions()
        {
            Reason = RefundReasons.RequestedByCustomer,
            PaymentIntent = orderModel.OrderHeader.PaymentIntentId,
        };

        RefundService service = new RefundService();
        Refund refund = await service.CreateAsync(refundOptions);
    }

    private async Task<Session> ConfigureStripe(DetailsOrderViewModel orderModel)
    {
        string domain = httpContextAccessor.GetDomain();
        SessionCreateOptions options = new SessionCreateOptions
        {
            LineItems = new List<SessionLineItemOptions>(),
            SuccessUrl = string.Format(FullSuccessUrlOrder, domain, orderModel.OrderHeader.Id),
            CancelUrl = string.Format(FullCancelUrlOrder, domain),
            Mode = "payment",
        };

        foreach (OrderDetailsViewModel orderDetailsModel in orderModel.OrderDetailsList)
        {
            options.LineItems.Add(new SessionLineItemOptions()
            {
                PriceData = new SessionLineItemPriceDataOptions()
                {
                    UnitAmountDecimal = orderDetailsModel.Count <= 50 ? Math.Round(orderDetailsModel.Book.Price * 100) :
                        orderDetailsModel.Count is > 50 and <= 100 ?
                            Math.Round((orderDetailsModel.Book.Price * 100) - ((orderDetailsModel.Book.Price * 100) * 0.1M)) :
                            Math.Round((orderDetailsModel.Book.Price * 100) - ((orderDetailsModel.Book.Price * 100) * 0.2M)),
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions()
                    {
                        Name = orderDetailsModel.Book.Title,
                    }
                },
                Quantity = orderDetailsModel.Count,
            });
        }

        SessionService service = new SessionService();
        return await service.CreateAsync(options);
    }
}