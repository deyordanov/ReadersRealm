namespace ReadersRealm.Web.Areas.Admin.Controllers;

using AngleSharp.Css.Dom;
using Common;
using Common.Exceptions.GeneralExceptions;
using Data.Models;
using Data.Models.Enums;
using Infrastructure.Settings.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using ReadersRealm.Services.Data.AuthorServices.Contracts;
using ReadersRealm.Services.Data.BookServices.Contracts;
using ReadersRealm.Services.Data.CategoryServices.Contracts;
using ViewModels.Book;
using static Common.Constants.Constants.AreasConstants;
using static Common.Constants.Constants.BookConstants;
using static Common.Constants.Constants.ErrorConstants;
using static Common.Constants.Constants.ImageConstants;
using static Common.Constants.Constants.RolesConstants;
using static Common.Constants.Constants.SharedConstants;
using static Common.Constants.ExceptionMessages.ImageExceptionMessages;
using static Common.Constants.ValidationMessageConstants.AuthorValidationMessages;
using static Common.Constants.ValidationMessageConstants.BookValidationMessages;
using static Common.Constants.ValidationMessageConstants.CategoryValidationMessages;


[Area(Admin)]
public class BookController : BaseController
{
    private readonly IBookRetrievalService _bookRetrievalService;
    private readonly IBookCrudService _bookCrudService;
    private readonly IAuthorRetrievalService _authorRetrievalService;
    private readonly ICategoryRetrievalService _categoryRetrievalService;
    private readonly IMongoDbSettings _mongoDbSettings;
    private readonly GridFSBucket _gridFsBucket;

    public BookController(
        IBookRetrievalService bookRetrievalService, 
        IBookCrudService bookCrudService, 
        IAuthorRetrievalService authorRetrievalService, 
        ICategoryRetrievalService categoryRetrievalService,
        IMongoDbSettings mongoDbSettings)
    {
        this._bookRetrievalService = bookRetrievalService;
        this._bookCrudService = bookCrudService;
        this._authorRetrievalService = authorRetrievalService;
        this._categoryRetrievalService = categoryRetrievalService;
        this._mongoDbSettings = mongoDbSettings;

        MongoClient client = new MongoClient(_mongoDbSettings.ConnectionString);
        IMongoDatabase mongoDatabase = client.GetDatabase(_mongoDbSettings.DatabaseName);
        this._gridFsBucket = new GridFSBucket(mongoDatabase);
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

        bool authorExists = await this
            ._authorRetrievalService
            .AuthorExistsAsync(bookModel.AuthorId);
        if (!authorExists)
        {
            ModelState.AddModelError(AuthorId, AuthorDoesNotExistMessage);
        }

        bool categoryExists = await this
            ._categoryRetrievalService
            .CategoryExistsAsync(bookModel.CategoryId);
        if (!categoryExists)
        {
            ModelState.AddModelError(CategoryId, CategoryDoesNotExistMessage);
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

        bookModel.ImageId = await this.UploadImageAsync(file);

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
        bool authorExists = await this
            ._authorRetrievalService
            .AuthorExistsAsync(bookModel.AuthorId);
        if (!authorExists)
        {
            ModelState.AddModelError(AuthorId, AuthorDoesNotExistMessage);
        }

        bool categoryExists = await this
            ._categoryRetrievalService
            .CategoryExistsAsync(bookModel.CategoryId);
        if (!categoryExists)
        {
            ModelState.AddModelError(CategoryId, CategoryDoesNotExistMessage);
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

        if (bookModel.ImageId != null)
        {
            await this._gridFsBucket.DeleteAsync(ObjectId.Parse(bookModel.ImageId));
        }

        bookModel.ImageId = await this.UploadImageAsync(file);

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
        await this._gridFsBucket.DeleteAsync(ObjectId.Parse(bookModel.ImageId));

        await this
            ._bookCrudService
            .DeleteBookAsync(bookModel);

        TempData[Success] = BookHasBeenSuccessfullyDeleted;

        return RedirectToAction(nameof(Index), nameof(Book));
    }

    private async Task<string> UploadImageAsync(IFormFile? file)
    {
        ObjectId? imageId;
        if (file == null || file.Length == 0)
        {
            FilterDefinition<GridFSFileInfo> filter = Builders<GridFSFileInfo>.Filter.Eq(info => info.Filename, DefaultImage);

            using IAsyncCursor<GridFSFileInfo> cursor = await this._gridFsBucket.FindAsync(filter);

            GridFSFileInfo fileInfo = await cursor.FirstOrDefaultAsync();

            imageId = fileInfo.Id;
        }
        else
        {
            await using var stream = file.OpenReadStream();

            imageId = await this._gridFsBucket.UploadFromStreamAsync(file.FileName, stream);
        }

        return imageId.ToString() ?? string.Empty;
    }

    public async Task<IActionResult> GetImageAsync(string id)
    {
        if (!ObjectId.TryParse(id, out var imageId))
        {
            throw new InvalidImageIdException(string.Format(InvalidImageIdExceptionMessage, id, nameof(this.GetImageAsync)));
        }

        var bytes = await this._gridFsBucket.DownloadAsBytesAsync(imageId);

        return File(bytes, ContentType);
    }
}