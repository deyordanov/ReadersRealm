namespace ReadersRealm.Web.Areas.Customer.Controllers;

using Common;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadersRealm.Areas;
using ReadersRealm.ViewModels.Book;
using ReadersRealm.ViewModels.ShoppingCart;
using System.Diagnostics;
using Infrastructure.Extensions;
using Services.Data.Contracts;
using ViewModels;
using static Common.Constants.Constants.Areas;
using static Common.Constants.Constants.SessionKeys;
using static Common.Constants.Constants.Shared;
using static Common.Constants.Constants.ShoppingCart;

[Area(Customer)]
public class HomeController : BaseController
{
    private readonly IBookService _bookService;
    private readonly IShoppingCartService _shoppingCartService;

    public HomeController(IBookService bookService, IShoppingCartService shoppingCartService)
    {
        _bookService = bookService;
        _shoppingCartService = shoppingCartService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Index(int pageIndex, string? searchTerm)
    {
        if (User.IsAuthenticated())
        {
            await SetShoppingCartItemsCountInSession();
        }

        PaginatedList<AllBooksViewModel> allBooks = await _bookService
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

        DetailsBookViewModel bookModel = await _bookService
            .GetBookForDetailsAsync((Guid)id);

        ShoppingCartViewModel shoppingCartModel = _shoppingCartService
            .GetShoppingCart(bookModel);

        return View(shoppingCartModel);
    }

    [HttpPost]
    public async Task<IActionResult> Details(ShoppingCartViewModel shoppingCartModel)
    {
        if (!ModelState.IsValid)
        {
            DetailsBookViewModel bookModel = await _bookService
                .GetBookForDetailsAsync(shoppingCartModel.BookId);

            shoppingCartModel.Book = bookModel;

            return View(shoppingCartModel);
        }

        string userId = User.GetId();
        shoppingCartModel.ApplicationUserId = userId;

        bool shoppingCartExists = await _shoppingCartService
            .ShoppingCartExistsAsync(userId, shoppingCartModel.BookId);


        if (!shoppingCartExists)
        {
            await _shoppingCartService
                .CreateShoppingCartAsync(shoppingCartModel);
        }
        else
        {
            await _shoppingCartService
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

        int itemsCount = await _shoppingCartService
            .GetShoppingCartCountByApplicationUserIdAsync(userId);

        HttpContext.Session.SetInt32(ShoppingCartSessionKey, itemsCount);
    }
}