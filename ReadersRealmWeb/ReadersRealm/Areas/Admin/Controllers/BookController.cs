using Microsoft.AspNetCore.Mvc;

namespace ReadersRealm.Areas.Admin.Controllers;

using Data.Models;
using Services.Contracts;
using ViewModels.Book;
using ViewModels.Category;
using Web.ViewModels.Author;
using Web.ViewModels.Book;

[Area("Admin")]
public class BookController : Controller
{
    private IBookService bookService;
    private IAuthorService authorService;
    private ICategoryService categoryService;

    public BookController(IBookService bookService, 
        IAuthorService authorService,
        ICategoryService categoryService)
    {
        this.bookService = bookService;
        this.authorService = authorService;
        this.categoryService = categoryService;
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
        List<AllAuthorsListViewModel> authorsList = await this
            .authorService
            .GetAllListAsync();

        List<AllCategoriesListViewModel> categoriesList = await this
            .categoryService
            .GetAllListAsync();

        CreateBookViewModel bookModel = new CreateBookViewModel()
        {
            Title = "",
            ISBN = "",
            AuthorsList = authorsList,
            CategoriesList = categoriesList
        };

        return View(bookModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBookViewModel bookModel)
    {
        if (!ModelState.IsValid)
        {
            return View(bookModel);
        }

        await
            bookService
                .CreateBookAsync(bookModel);

        TempData["Success"] = "Successfully created a new book!";

        return RedirectToAction(nameof(Index), nameof(Book));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Book? book = await this
            .bookService
            .GetBookByIdWithNavPropertiesAsync((Guid)id);

        if (book == null)
        {
            return NotFound();
        }

        List<AllAuthorsListViewModel> authorsList = await this
            .authorService
            .GetAllListAsync();

        List<AllCategoriesListViewModel> categoriesList = await this
            .categoryService
            .GetAllListAsync();

        EditBookViewModel bookModel = new EditBookViewModel()
        {
            Id = book.Id,
            ISBN = book.ISBN,
            Title = book.Title,
            BookCover = book.BookCover,
            Description = book.Description,
            Pages = book.Pages,
            Price = book.Price,
            Used = book.Used,
            Author = book.Author,
            AuthorId = book.AuthorId,
            Category = book.Category,
            CategoryId = book.CategoryId,
            AuthorsList = authorsList,
            CategoriesList = categoriesList
        };

        return View(bookModel);
    }

    public async Task<IActionResult> Edit(EditBookViewModel bookModel)
    {
        if (!ModelState.IsValid)
        {
            return View(bookModel);
        }

        await this
            .bookService
            .EditBookAsync(bookModel);

        TempData["Success"] = "Book has been edited successfully!";

        return RedirectToAction(nameof(Index), nameof(Book));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Book? book = await this
            .bookService
                .GetBookByIdWithNavPropertiesAsync((Guid)id);

        if (book == null)
        {
            return NotFound();
        }

        DeleteBookViewModel bookModel = new DeleteBookViewModel()
        {
            Id = book.Id,
            ISBN = book.ISBN,
            Title = book.Title,
            BookCover = book.BookCover,
            Description = book.Description,
            Pages = book.Pages,
            Price = book.Price,
            Used = book.Used,
            Author = book.Author,
            AuthorId = book.AuthorId,
            Category = book.Category,
            CategoryId = book.CategoryId,
        };

        return View(bookModel);
    }

    public async Task<IActionResult> Delete(DeleteBookViewModel bookModel)
    {
        await this
            .bookService
            .DeleteBookAsync(bookModel);

        TempData["Success"] = "Book has been deleted successfully!";

        return RedirectToAction(nameof(Index), nameof(Book));
    }
}