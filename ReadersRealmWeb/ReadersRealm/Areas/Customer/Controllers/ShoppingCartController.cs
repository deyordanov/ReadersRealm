using Microsoft.AspNetCore.Mvc;

namespace ReadersRealm.Areas.Customer.Controllers;

using Data.Models;
using Extensions.ClaimsPrincipal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Services.Contracts;
using ViewModels.ApplicationUser;
using ViewModels.ShoppingCart;
using Web.ViewModels.OrderDetails;
using static Common.Constants.Constants.Shared;
using static Common.Constants.Constants.ShoppingCart;
using static Common.Constants.Constants.Roles;
using static Common.Constants.Constants.OrderHeader;

[Area("Customer")]
[Authorize]
public class ShoppingCartController : Controller
{
    private readonly IShoppingCartService shoppingCartService;
    private readonly IApplicationUserService applicationUserService;
    private readonly IOrderHeaderService orderHeaderService;
    private readonly IOrderDetailsService orderDetailsService;

    public ShoppingCartController(
        IShoppingCartService shoppingCartService, 
        IApplicationUserService applicationUserService,
        IOrderHeaderService orderHeaderService,
        IOrderDetailsService orderDetailsService)
    {
        this.shoppingCartService = shoppingCartService;
        this.applicationUserService = applicationUserService;
        this.orderHeaderService = orderHeaderService;
        this.orderDetailsService = orderDetailsService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        string userId = User.GetId();

        AllShoppingCartsListViewModel shoppingCartModel = await this
            .shoppingCartService
            .GetAllListAsync(userId);

        return View(shoppingCartModel);
    }

    [HttpGet]
    public async Task<IActionResult> Summary()
    {
        string userId = User.GetId();

        AllShoppingCartsListViewModel shoppingCartModel = await this
            .shoppingCartService
            .GetAllListAsync(userId);

        return View(shoppingCartModel);
    }

    [HttpPost]
    public async Task<IActionResult> Summary(AllShoppingCartsListViewModel shoppingCartModel)
    {
        string userId = User.GetId();

        shoppingCartModel = await this
            .shoppingCartService
            .GetAllListAsync(userId);

        if (!ModelState.IsValid)
        {
            return View(shoppingCartModel);
        }
    
        shoppingCartModel.OrderHeader.OrderDate = DateTime.Now;

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
            .applicationUserService
            .UpdateApplicationUserAsync(applicationUserModel);

        if (User.IsInRole(CompanyRole))
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

        Guid orderHeaderId = await this
            .orderHeaderService
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
                .orderDetailsService
                .CreateOrderDetailsAsync(orderDetailsModel);
        }

        return RedirectToAction(nameof(OrderConfirmation), nameof(ShoppingCart)); 
    }

    [HttpGet]
    public IActionResult OrderConfirmation()
    {
        return View(12);
    }

    [HttpGet]
    public async Task<IActionResult> IncreaseQuantity(Guid? id)
    {
        if (id == null || id == Guid.Empty)
        {
            return NotFound();
        }

        await this
            .shoppingCartService
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
            .shoppingCartService
            .DecreaseQuantityForShoppingCartAsync((Guid)id);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        await this
            .shoppingCartService
            .DeleteShoppingCartAsync(id);

        TempData[Success] = ShoppingCartItemHasBeenDeletedSuccessfully;

        return RedirectToAction(nameof(Index));
    }
}