using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ReadersRealm.Areas.Customer.Controllers;

using Data.Models;
using Extensions.ClaimsPrincipal;
using Microsoft.AspNetCore.Authorization;
using Services.Contracts;
using ViewModels.Book;
using ViewModels.ShoppingCart;
using Web.ViewModels;
using Web.ViewModels.Book;
using static Common.Constants.Constants.Shared;
using static Common.Constants.Constants.ShoppingCart;

[Area("Customer")]
public class HomeController : Controller
{
    private readonly IBookService bookService;
    private readonly IShoppingCartService shoppingCartService;

    public HomeController(IBookService bookService, IShoppingCartService shoppingCartService)
    {
        this.bookService = bookService;
        this.shoppingCartService = shoppingCartService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int pageIndex, string? searchTerm)
    {
        IEnumerable<AllBooksViewModel> allBooks = await this
            .bookService
            .GetAllAsync(pageIndex, 8, searchTerm);

        return View(allBooks);
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null || id == Guid.Empty)
        {
            return NotFound();
        }

        DetailsBookViewModel bookModel = await this
            .bookService
            .GetBookForDetailsAsync((Guid)id);

        ShoppingCartViewModel shoppingCartModel = this
            .shoppingCartService
            .GetShoppingCart(bookModel);

        return View(shoppingCartModel);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Details(ShoppingCartViewModel shoppingCartModel)
    {
        if (!ModelState.IsValid)
        {
            DetailsBookViewModel bookModel = await this
                .bookService
                .GetBookForDetailsAsync(shoppingCartModel.BookId);

            shoppingCartModel.Book = bookModel;

            return View(shoppingCartModel);
        }

        string userId = User.GetId();
        shoppingCartModel.ApplicationUserId = userId;

        bool shoppingCartExists = await this.shoppingCartService.ShoppingCartExistsAsync(userId, shoppingCartModel.BookId);
        if (!shoppingCartExists)
        {
            await this
                .shoppingCartService
                .CreateShoppingCartAsync(shoppingCartModel);
        }
        else
        {
            await this
                .shoppingCartService
                .UpdateShoppingCartCountAsync(shoppingCartModel);
        }

        TempData[Success] = ShoppingCartItemsHaveBeenAddedSuccessfully;

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}