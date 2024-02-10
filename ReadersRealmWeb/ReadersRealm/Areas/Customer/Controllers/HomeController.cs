namespace ReadersRealm.Areas.Customer.Controllers;

using Extensions.ClaimsPrincipal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System.Diagnostics;
using Common;
using ViewModels.Book;
using ViewModels.ShoppingCart;
using Web.ViewModels;
using static Common.Constants.Constants.Areas;
using static Common.Constants.Constants.Shared;
using static Common.Constants.Constants.ShoppingCart;

[Area(Customer)]
public class HomeController : BaseController
{
    private readonly IBookService _bookService;
    private readonly IShoppingCartService _shoppingCartService;

    public HomeController(IBookService bookService, IShoppingCartService shoppingCartService)
    {
        this._bookService = bookService;
        this._shoppingCartService = shoppingCartService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Index(int pageIndex, string? searchTerm)
    {
        PaginatedList<AllBooksViewModel> allBooks = await this
            ._bookService
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
            ._bookService
            .GetBookForDetailsAsync((Guid)id);

        ShoppingCartViewModel shoppingCartModel = this
            ._shoppingCartService
            .GetShoppingCart(bookModel);

        return View(shoppingCartModel);
    }

    [HttpPost]
    public async Task<IActionResult> Details(ShoppingCartViewModel shoppingCartModel)
    {
        if (!ModelState.IsValid)
        {
            DetailsBookViewModel bookModel = await this
                ._bookService
                .GetBookForDetailsAsync(shoppingCartModel.BookId);

            shoppingCartModel.Book = bookModel;

            return View(shoppingCartModel);
        }

        string userId = User.GetId();
        shoppingCartModel.ApplicationUserId = userId;

        bool shoppingCartExists = await this._shoppingCartService.ShoppingCartExistsAsync(userId, shoppingCartModel.BookId);
        if (!shoppingCartExists)
        {
            await this
                ._shoppingCartService
                .CreateShoppingCartAsync(shoppingCartModel);
        }
        else
        {
            await this
                ._shoppingCartService
                .UpdateShoppingCartCountAsync(shoppingCartModel);
        }

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
}