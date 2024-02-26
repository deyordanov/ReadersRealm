namespace ReadersRealm.Web.Areas.Admin.Controllers;

using Common;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadersRealm.Services.Data.AuthorServices.Contracts;
using ReadersRealm.Services.Data.BookServices.Contracts;
using ReadersRealm.Services.Data.CategoryServices.Contracts;
using ViewModels.Book;
using static Common.Constants.Constants.Areas;
using static Common.Constants.Constants.Book;
using static Common.Constants.Constants.Error;
using static Common.Constants.Constants.Roles;
using static Common.Constants.Constants.Shared;
using static Common.Constants.ValidationMessageConstants.Book;


[Area(Admin)]
public class BookController : BaseController
{
    private readonly IBookRetrievalService _bookRetrievalService;
    private readonly IBookCrudService _bookCrudService;
    private readonly IAuthorRetrievalService _authorRetrievalService;
    private readonly ICategoryRetrievalService _categoryRetrievalService;
    private readonly IWebHostEnvironment _webHost;

    public BookController(
        IWebHostEnvironment webHost, 
        IBookRetrievalService bookRetrievalService, 
        IBookCrudService bookCrudService, 
        IAuthorRetrievalService authorRetrievalService, 
        ICategoryRetrievalService categoryRetrievalService)
    {
        this._webHost = webHost;
        this._bookRetrievalService = bookRetrievalService;
        this._bookCrudService = bookCrudService;
        this._authorRetrievalService = authorRetrievalService;
        this._categoryRetrievalService = categoryRetrievalService;
    }

    [HttpGet]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Index(int pageIndex, string? searchTerm)
    {
        PaginatedList<AllBooksViewModel> allBooks = await this
            ._bookRetrievalService
            .GetAllAsync(pageIndex ,5, searchTerm);

        ViewBag.PrevDisabled = !allBooks.HasPreviousPage;
        ViewBag.NextDisabled = !allBooks.HasNextPage;
        ViewBag.ControllerName = nameof(Book);
        ViewBag.ActionName = nameof(Index);

        ViewBag.SearchTerm = searchTerm ?? string.Empty;

        return View(allBooks);
    }

    [HttpGet]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Create()
    {
        CreateBookViewModel bookModel = await this
            ._bookRetrievalService
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
                ._authorRetrievalService
                .GetAllListAsync();

            bookModel.CategoriesList = await this
                ._categoryRetrievalService
                .GetAllListAsync();

            return View(bookModel);
        }

        if (file != null)
        {
            string fileName = await UploadImageAsync(file, bookModel.ImageUrl);

            bookModel.ImageUrl = PathToSaveImage + fileName;
        }

        await this
                ._bookCrudService
                .CreateBookAsync(bookModel);

        TempData[Success] = BookHasBeenSuccessfullyCreated;

        return RedirectToAction(nameof(Index), nameof(Book));
    }

    [HttpGet]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Edit(Guid? id, int pageIndex, string? searchTerm)
    {
        if (id is not { } bookId || id == Guid.Empty)
        {
            return RedirectToAction(ErrorPageNotFoundAction, nameof(Error));
        }

        EditBookViewModel bookModel = await this
            ._bookRetrievalService
            .GetBookForEditAsync(bookId);

        ViewBag.PageIndex = pageIndex;
        ViewBag.SearchTerm = searchTerm!;

        return View(bookModel);
    }

    [HttpPost]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Edit(EditBookViewModel bookModel, IFormFile? file, int pageIndex, string? searchTerm)
    {
        if (!ModelState.IsValid)
        {
            bookModel.AuthorsList = await this
                ._authorRetrievalService
                .GetAllListAsync();

            bookModel.CategoriesList = await this
                ._categoryRetrievalService
                .GetAllListAsync();

            return View(bookModel);
        }

        if (file != null)
        {
            string fileName = await UploadImageAsync(file, bookModel.ImageUrl);

            bookModel.ImageUrl = PathToSaveImage + fileName;
        }

        await this
            ._bookCrudService
            .EditBookAsync(bookModel);

        TempData[Success] = BookHasBeenSuccessfullyEdited;

        return RedirectToAction(nameof(Index), nameof(Book), new { pageIndex = pageIndex, searchTerm = searchTerm });
    }

    [HttpGet]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id is not { } bookId || id == Guid.Empty)
        {
            return RedirectToAction(ErrorPageNotFoundAction, nameof(Error));
        }

        DeleteBookViewModel bookModel = await this
            ._bookRetrievalService
            .GetBookForDeleteAsync(bookId);

        return View(bookModel);
    }

    [HttpPost]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Delete(DeleteBookViewModel bookModel)
    {
        DeleteImageIfPresent(bookModel.ImageUrl, _webHost.WebRootPath);

        await this
            ._bookCrudService
            .DeleteBookAsync(bookModel);

        TempData[Success] = BookHasBeenSuccessfullyDeleted;

        return RedirectToAction(nameof(Index), nameof(Book));
    }

    private async Task<string> UploadImageAsync(IFormFile file, string? imageUrl)
    {
        string wwwRootPath = _webHost.WebRootPath;
        string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        string bookPath = Path.Combine(wwwRootPath, PathToSaveImage.TrimStart('\\'));

        DeleteImageIfPresent(imageUrl, wwwRootPath);

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