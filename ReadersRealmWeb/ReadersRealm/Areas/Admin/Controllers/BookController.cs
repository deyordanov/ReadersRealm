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
    private readonly IWebHostEnvironment webHost;

    public BookController(
        IBookService bookService, 
        ICategoryService categoryService, 
        IAuthorService authorService,
        IWebHostEnvironment webHost)
    {
        this.bookService = bookService;
        this.categoryService = categoryService;
        this.authorService = authorService;
        this.webHost = webHost; 
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
    public async Task<IActionResult> Create(CreateBookViewModel bookModel, IFormFile? file)
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

        if (file != null)
        {
            string fileName = await this.UploadImageAsync(file, bookModel.ImageUrl);

            bookModel.ImageUrl = @"\images\book\" + fileName;
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
    public async Task<IActionResult> Edit(EditBookViewModel bookModel, IFormFile? file)
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

        if (file != null)
        {
            string fileName = await this.UploadImageAsync(file, bookModel.ImageUrl);

            bookModel.ImageUrl = @"\images\book\" + fileName;
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

    private async Task<string> UploadImageAsync(IFormFile file, string? imageUrl)
    {
        string wwwRootPath = webHost.WebRootPath;
        string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        string bookPath = Path.Combine(wwwRootPath, @"images\book");

        if (!string.IsNullOrWhiteSpace(imageUrl))
        {
            string oldImagePath = Path.Combine(wwwRootPath, imageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
        }

        await using (FileStream stream = new FileStream(Path.Combine(bookPath, fileName), FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return fileName;
    }
}