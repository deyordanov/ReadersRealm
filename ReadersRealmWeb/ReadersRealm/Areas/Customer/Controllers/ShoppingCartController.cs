namespace ReadersRealm.Areas.Customer.Controllers;

using Data.Models;
using Extensions.ClaimsPrincipal;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using ViewModels.ApplicationUser;
using ViewModels.ShoppingCart;
using Web.ViewModels.OrderDetails;
using static Common.Constants.Constants.Areas;
using static Common.Constants.Constants.OrderHeader;
using static Common.Constants.Constants.Roles;
using static Common.Constants.Constants.Shared;
using static Common.Constants.Constants.ShoppingCart;

[Area(Customer)]
public class ShoppingCartController : BaseController
{
    private readonly IShoppingCartService _shoppingCartService;
    private readonly IApplicationUserService _applicationUserService;
    private readonly IOrderHeaderService _orderHeaderService;
    private readonly IOrderDetailsService _orderDetailsService;

    public ShoppingCartController(
        IShoppingCartService shoppingCartService, 
        IApplicationUserService applicationUserService,
        IOrderHeaderService orderHeaderService,
        IOrderDetailsService orderDetailsService)
    {
        this._shoppingCartService = shoppingCartService;
        this._applicationUserService = applicationUserService;
        this._orderHeaderService = orderHeaderService;
        this._orderDetailsService = orderDetailsService;
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
            ._applicationUserService
            .UpdateApplicationUserAsync(applicationUserModel);

        shoppingCartModel = await this
            ._shoppingCartService
            .GetAllListAsync(userId);

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

        return RedirectToAction(nameof(OrderConfirmation), nameof(ShoppingCart)); 
    }

    [HttpGet]
    public IActionResult OrderConfirmation()
    {
        return View();
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
}