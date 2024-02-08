namespace ReadersRealm.Areas.Customer.Controllers;

using Data.Models;
using Extensions.ClaimsPrincipal;
using Microsoft.AspNetCore.Mvc;
using ReadersRealm.Extensions.HttpContextAccessor;
using Services.Contracts;
using Stripe.Checkout;
using ViewModels.ApplicationUser;
using ViewModels.ShoppingCart;
using Web.ViewModels.OrderDetails;
using static Common.Constants.Constants.Areas;
using static Common.Constants.Constants.OrderHeader;
using static Common.Constants.Constants.Roles;
using static Common.Constants.Constants.Shared;
using static Common.Constants.Constants.ShoppingCart;
using static Common.Constants.Constants.StripeSettings;
using Stripe.Checkout;
using ViewModels.OrderHeader;

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
            Session session = await service.CreateAsync(options);

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
                await this
                    ._orderHeaderService
                    .UpdateOrderHeaderPaymentIntentIdAsync(orderHeaderModel.Id, session.Id, session.PaymentIntentId);

                await this
                    ._orderHeaderService
                    .UpdateOrderHeaderStatusAsync(orderHeaderModel.Id, OrderStatusApproved, PaymentStatusApproved);
            }
        }

        string applicationUserId = this.User.GetId();
        await this
            ._shoppingCartService
            .DeleteAllShoppingCartsApplicationUserIdAsync(applicationUserId);

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