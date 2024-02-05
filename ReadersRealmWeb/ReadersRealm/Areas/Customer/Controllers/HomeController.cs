using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ReadersRealm.Areas.Customer.Controllers;

using Services.Contracts;
using ViewModels.Book;
using Web.ViewModels;
using Web.ViewModels.Book;

[Area("Customer")]
public class HomeController : Controller
{
    private IBookService bookService;

    public HomeController(IBookService bookService)
    {
        this.bookService = bookService;
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

        return View(bookModel);
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