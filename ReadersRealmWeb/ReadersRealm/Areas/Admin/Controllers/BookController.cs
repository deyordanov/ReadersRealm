using Microsoft.AspNetCore.Mvc;

namespace ReadersRealm.Areas.Admin.Controllers;

using Data.Models;
using Services.Contracts;
using ViewModels.Book;
using Web.ViewModels.Book;
using static Common.Constants.Book;
using static Common.Constants.Shared;

[Area("Admin")]
public class BookController : Controller
{
    private readonly IBookService bookService;
    private readonly ICategoryService categoryService;
    private readonly IAuthorService authorService;

    public BookController(
        IBookService bookService, 
        ICategoryService categoryService, 
        IAuthorService authorService)
    {
        this.bookService = bookService;
        this.categoryService = categoryService;
        this.authorService = authorService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        IEnumerable<AllBooksViewModel> allBooks = await this
            .bookService
            .GetAllAsync();

        return View(allBooks);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        CreateBookViewModel bookModel = await this
            .bookService
            .GetBookForCreateAsync();

        return View(bookModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBookViewModel bookModel)
    {
        if (bookModel.AuthorId == Guid.Empty)
        {
            ModelState.AddModelError("AuthorId", "Author is required!");
        }

        if (bookModel.CategoryId == 0)
        {
            ModelState.AddModelError("CategoryId", "Category is required!");
        }

        if (!ModelState.IsValid)
        {
            bookModel.AuthorsList = await this
                .authorService
                .GetAllListAsync();

            bookModel.CategoriesList = await this
                .categoryService
                .GetAllListAsync();

            return View(bookModel);
        }

        await
            bookService
                .CreateBookAsync(bookModel);

        TempData[Success] = BookHasBeenSuccessfullyCreated;

        return RedirectToAction(nameof(Index), nameof(Book));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        EditBookViewModel bookModel = await this
            .bookService
            .GetBookForEditAsync((Guid)id);

        return View(bookModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditBookViewModel bookModel)
    {
        if (!ModelState.IsValid)
        {
            bookModel.AuthorsList = await this
                .authorService
                .GetAllListAsync();

            bookModel.CategoriesList = await this
                .categoryService
                .GetAllListAsync();

            return View(bookModel);
        }

        await this
            .bookService
            .EditBookAsync(bookModel);

        TempData[Success] = BookHasBeenSuccessfullyEdited;

        return RedirectToAction(nameof(Index), nameof(Book));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        DeleteBookViewModel bookModel = await this
            .bookService
            .GetBookForDeleteAsync((Guid)id);

        return View(bookModel);
    }

    [HttpPost]

    public async Task<IActionResult> Delete(DeleteBookViewModel bookModel)
    {
        await this
            .bookService
            .DeleteBookAsync(bookModel);

        TempData[Success] = BookHasBeenSuccessfullyDeleted;

        return RedirectToAction(nameof(Index), nameof(Book));
    }
}