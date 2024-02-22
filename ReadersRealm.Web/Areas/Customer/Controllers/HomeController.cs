namespace ReadersRealm.Web.Areas.Customer.Controllers;

using System.Diagnostics;
using Common;
using Duende.IdentityServer.Extensions;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Data.BookServices.Contracts;
using Services.Data.ShoppingCartServices.Contracts;
using ViewModels;
using ViewModels.Book;
using ViewModels.ShoppingCart;
using static Common.Constants.Constants.Areas;
using static Common.Constants.Constants.SessionKeys;
using static Common.Constants.Constants.Shared;
using static Common.Constants.Constants.ShoppingCart;

[Area(Customer)]
public class HomeController : BaseController
{
    private readonly IShoppingCartCrudService _shoppingCartCrudService;
    private readonly IShoppingCartRetrievalService _shoppingCartRetrievalService;
    private readonly IBookRetrievalService _bookRetrievalService;

    public HomeController(
        IShoppingCartCrudService shoppingCartCrudService, 
        IShoppingCartRetrievalService shoppingCartRetrievalService, 
        IBookRetrievalService bookRetrievalService)
    {
        this._shoppingCartCrudService = shoppingCartCrudService;
        this._shoppingCartRetrievalService = shoppingCartRetrievalService;
        this._bookRetrievalService = bookRetrievalService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Index(int pageIndex, string? searchTerm)
    {
        if (User.IsAuthenticated())
        {
            await SetShoppingCartItemsCountInSession();
        }

        PaginatedList<AllBooksViewModel> allBooks = await this
            ._bookRetrievalService
            .GetAllAsync(pageIndex, 8, searchTerm);


        return View(allBooks);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null || id == Guid.Empty)
        {
            return NotFound();
        }

        DetailsBookViewModel bookModel = await this
            ._bookRetrievalService
            .GetBookForDetailsAsync((Guid)id);

        ShoppingCartViewModel shoppingCartModel = this
            ._shoppingCartRetrievalService
            .GetShoppingCart(bookModel);

        return View(shoppingCartModel);
    }

    [HttpPost]
    public async Task<IActionResult> Details(ShoppingCartViewModel shoppingCartModel)
    {
        if (!ModelState.IsValid)
        {
            DetailsBookViewModel bookModel = await this
                ._bookRetrievalService
                .GetBookForDetailsAsync(shoppingCartModel.BookId);

            shoppingCartModel.Book = bookModel;

            return View(shoppingCartModel);
        }

        string userId = User.GetId();
        shoppingCartModel.ApplicationUserId = userId;

        bool shoppingCartExists = await this
            ._shoppingCartRetrievalService
            .ShoppingCartExistsAsync(userId, shoppingCartModel.BookId);


        if (!shoppingCartExists)
        {
            await this
                ._shoppingCartCrudService
                .CreateShoppingCartAsync(shoppingCartModel);
        }
        else
        {
            await this
                ._shoppingCartCrudService
                .UpdateShoppingCartCountAsync(shoppingCartModel);
        }

        await SetShoppingCartItemsCountInSession();

            TempData[Success] = ShoppingCartItemsHaveBeenAddedSuccessfully;

        return RedirectToAction(nameof(Index));
    }

    [AllowAnonymous]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [AllowAnonymous]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private async Task SetShoppingCartItemsCountInSession()
    {
        string userId = User.GetId();

        int itemsCount = await this
            ._shoppingCartRetrievalService
            .GetShoppingCartCountByApplicationUserIdAsync(userId);

        HttpContext.Session.SetInt32(ShoppingCartSessionKey, itemsCount);
    }
}