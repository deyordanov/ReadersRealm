namespace ReadersRealm.Areas.Admin.Controllers;

using Common;
using Data.Models;
using Extensions.ClaimsPrincipal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Stripe;
using ViewModels.Order;
using static Common.Constants.Constants.Areas;
using static Common.Constants.Constants.Order;
using static Common.Constants.Constants.Roles;
using static Common.Constants.Constants.Shared;
using static Common.Constants.Constants.OrderHeader;

[Area(Admin)]
public class OrderController : BaseController
{
    private const string AuthorizeAdminAndEmployeeRoles = $"{AdminRole}, {EmployeeRole}";

    private readonly IOrderService _orderService;
    private readonly IOrderHeaderService _orderHeaderService;

    public OrderController(
        IOrderService orderService,
        IOrderHeaderService orderHeaderService)
    {
        this._orderService = orderService;
        this._orderHeaderService = orderHeaderService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int pageIndex, string? searchTerm)
    {
        PaginatedList<AllOrdersViewModel> allOrders;
        bool isUserAdminOrEmployee = User.IsInRole(AdminRole) || User.IsInRole(EmployeeRole);
        if (isUserAdminOrEmployee)
        {
            allOrders = await this
                ._orderService
                .GetAllAsync(pageIndex, 5, searchTerm);
        }
        else
        {
            string userId = User.GetId();
            allOrders = await this
                ._orderService
                .GetAllByUserIdAsync(pageIndex, 5, searchTerm, userId);
        }

        return View(allOrders);
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid? id, int pageIndex, string? searchTerm)
    {
        if (id == null || id == Guid.Empty)
        {
            return NotFound();
        }

        DetailsOrderViewModel orderModel = await this
            ._orderService
            .GetOrderForDetailsAsync((Guid)id);

        ViewBag.PageIndex = pageIndex;
        ViewBag.SearchTerm = searchTerm;

        return View(orderModel);
    }

    [HttpPost]
    [Authorize(Roles = AuthorizeAdminAndEmployeeRoles)]
    public async Task<IActionResult> Details(DetailsOrderViewModel orderModel, int pageIndex, string? searchTerm)
    {
        if (!ModelState.IsValid)
        {
            orderModel = await this
                ._orderService
                .GetOrderForDetailsAsync((Guid)orderModel.Id);

            return View(orderModel);
        }

        await this
            ._orderService
            .UpdateOrderAsync(orderModel);

        TempData[Success] = OrderHasSuccessfullyBeenUpdated;

        orderModel = await this
            ._orderService
            .GetOrderForDetailsAsync((Guid)orderModel.Id);

        return View(orderModel);
    }

    [HttpPost]
    [Authorize(Roles = AuthorizeAdminAndEmployeeRoles)]
    public async Task<IActionResult> StartProcessingOrder(Guid orderHeaderId, int pageIndex, string? searchTerm)
    {
        await this
            ._orderHeaderService
            .UpdateOrderHeaderStatusAsync(orderHeaderId, OrderStatusInProcess, null);

        TempData[Success] = OrderStatusHasSuccessfullyBeenUpdated;

        Guid orderId = await this
            ._orderService
            .GetOrderIdByOrderHeaderIdAsync(orderHeaderId);

        return RedirectToAction(nameof(Details), nameof(Order), new { id = orderId, pageIndex, searchTerm });
    }

    [HttpPost]
    [Authorize(Roles = AuthorizeAdminAndEmployeeRoles)]
    public async Task<IActionResult> ShipOrder(DetailsOrderViewModel orderModel, int pageIndex, string? searchTerm)
    {
        Guid orderId = await this
            ._orderService
            .GetOrderIdByOrderHeaderIdAsync(orderModel.OrderHeaderId);

        if (orderModel.OrderHeader.TrackingNumber == null ||
            orderModel.OrderHeader.Carrier == null)
        {
            TempData[Error] = OrderTrackingNumberAndCarrierAreNotSet;
            return RedirectToAction(nameof(Details), nameof(Order), new { id = orderId, pageIndex, searchTerm });
        }

        // await this
        //      ._orderHeaderService
        //      .UpdateOrderHeaderStatusAsync(orderModel.OrderHeaderId, OrderStatusShipped, null);

        orderModel.OrderHeader.OrderStatus = OrderStatusShipped;
        orderModel.OrderHeader.ShippingDate = DateTime.UtcNow;

        if (orderModel.OrderHeader.PaymentStatus == PaymentStatusDelayedPayment)
        {
            orderModel.OrderHeader.PaymentDueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30));
        }

        await this
            ._orderHeaderService
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
            await this.InitiateStripeRefund(orderModel);

            await this
                ._orderHeaderService
                .UpdateOrderHeaderStatusAsync(orderModel.OrderHeader.Id, OrderStatusCancelled, PaymentStatusRefunded);
        }
        else
        {
            await this
                ._orderHeaderService
                .UpdateOrderHeaderStatusAsync(orderModel.OrderHeader.Id, OrderStatusCancelled, PaymentStatusCancelled);
        }

        TempData[Success] = OrderStatusHasSuccessfullyBeenUpdated;

        Guid orderId = await this
            ._orderService
            .GetOrderIdByOrderHeaderIdAsync(orderModel.OrderHeader.Id);

        return RedirectToAction(nameof(Details), nameof(Order), new { id = orderId, pageIndex, searchTerm });
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
}