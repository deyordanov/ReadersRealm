namespace ReadersRealm.Areas.Admin.Controllers;

using Common;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using ViewModels.Book;
using Web.ViewModels.Book;
using static Common.Constants.Constants.Areas;
using static Common.Constants.Constants.Book;
using static Common.Constants.Constants.Roles;
using static Common.Constants.Constants.Shared;
using static Common.Constants.ValidationMessageConstants.Book;


[Area(Admin)]
public class BookController : BaseController
{
    private readonly IBookService _bookService;
    private readonly ICategoryService _categoryService;
    private readonly IAuthorService _authorService;
    private readonly IWebHostEnvironment _webHost;

    public BookController(
        IBookService bookService, 
        ICategoryService categoryService, 
        IAuthorService authorService,
        IWebHostEnvironment webHost)
    {
        this._bookService = bookService;
        this._categoryService = categoryService;
        this._authorService = authorService;
        this._webHost = webHost;
    }

    [HttpGet]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Index(int pageIndex, string? searchTerm)
    {
        PaginatedList<AllBooksViewModel> allBooks = await this
            ._bookService
            .GetAllAsync(pageIndex ,5, searchTerm);

        ViewBag.SearchTerm = searchTerm ?? "";

        return View(allBooks);
    }

    [HttpGet]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Create()
    {
        CreateBookViewModel bookModel = await this
            ._bookService
            .GetBookForCreateAsync();

        return View(bookModel);
    }

    [HttpPost]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Create(CreateBookViewModel bookModel, IFormFile? file)
    {
        if (bookModel.AuthorId == Guid.Empty)
        {
            ModelState.AddModelError(AuthorId, BookAuthorRequiredMessage);
        }

        if (bookModel.CategoryId == 0)
        {
            ModelState.AddModelError(CategoryId, BookCategoryRequiredMessage);
        }

        if (!ModelState.IsValid)
        {
            bookModel.AuthorsList = await this
                ._authorService
                .GetAllListAsync();

            bookModel.CategoriesList = await this
                ._categoryService
                .GetAllListAsync();

            return View(bookModel);
        }

        if (file != null)
        {
            string fileName = await this.UploadImageAsync(file, bookModel.ImageUrl);

            bookModel.ImageUrl = PathToSaveImage + fileName;
        }

        await
            _bookService
                .CreateBookAsync(bookModel);

        TempData[Success] = BookHasBeenSuccessfullyCreated;

        return RedirectToAction(nameof(Index), nameof(Book));
    }

    [HttpGet]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Edit(Guid? id, int pageIndex, string? searchTerm)
    {
        if (id == null || id == Guid.Empty)
        {
            return NotFound();
        }

        EditBookViewModel bookModel = await this
            ._bookService
            .GetBookForEditAsync((Guid)id);

        ViewBag.PageIndex = pageIndex;
        ViewBag.SearchTerm = searchTerm;

        return View(bookModel);
    }

    [HttpPost]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Edit(EditBookViewModel bookModel, IFormFile? file, int pageIndex, string? searchTerm)
    {
        if (!ModelState.IsValid)
        {
            bookModel.AuthorsList = await this
                ._authorService
                .GetAllListAsync();

            bookModel.CategoriesList = await this
                ._categoryService
                .GetAllListAsync();

            return View(bookModel);
        }

        if (file != null)
        {
            string fileName = await this.UploadImageAsync(file, bookModel.ImageUrl);

            bookModel.ImageUrl = PathToSaveImage + fileName;
        }

        await this
            ._bookService
            .EditBookAsync(bookModel);

        TempData[Success] = BookHasBeenSuccessfullyEdited;

        return RedirectToAction(nameof(Index), nameof(Book), new { pageIndex = pageIndex, searchTerm = searchTerm });
    }

    [HttpGet]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null || id == Guid.Empty)
        {
            return NotFound();
        }

        DeleteBookViewModel bookModel = await this
            ._bookService
            .GetBookForDeleteAsync((Guid)id);

        return View(bookModel);
    }

    [HttpPost]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Delete(DeleteBookViewModel bookModel)
    {
        this.DeleteImageIfPresent(bookModel.ImageUrl, _webHost.WebRootPath);

        await this
            ._bookService
            .DeleteBookAsync(bookModel);

        TempData[Success] = BookHasBeenSuccessfullyDeleted;

        return RedirectToAction(nameof(Index), nameof(Book));
    }

    private async Task<string> UploadImageAsync(IFormFile file, string? imageUrl)
    {
        string wwwRootPath = _webHost.WebRootPath;
        string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        string bookPath = Path.Combine(wwwRootPath, PathToSaveImage);

        this.DeleteImageIfPresent(imageUrl, wwwRootPath);

        await using FileStream stream = new FileStream(Path.Combine(bookPath, fileName), FileMode.Create);
        await file.CopyToAsync(stream);

        return fileName;
    }

    private void DeleteImageIfPresent(string? imageUrl, string wwwRootPath)
    {
        if (!string.IsNullOrWhiteSpace(imageUrl))
        {
            string oldImagePath = Path.Combine(wwwRootPath, imageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
        }
    }
}