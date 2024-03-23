namespace ReadersRealm.Web.Areas.Admin.Controllers;

using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadersRealm.Data.Models;
using Services.Data.AuthorServices.Contracts;
using ViewModels.Author;
using static Common.Constants.Constants.AreasConstants;
using static Common.Constants.Constants.RolesConstants;
using static Common.Constants.Constants.SharedConstants;
using static Common.Constants.Constants.AuthorConstants;
using static Common.Constants.Constants.ErrorConstants;
using AngleSharp.Css.Dom;

[Area(Admin)]
public class AuthorController : BaseController
{
    private readonly IAuthorRetrievalService _authorRetrievalService;
    private readonly IAuthorCrudService _authorCrudService;

    public AuthorController(
        IAuthorRetrievalService authorRetrievalService, 
        IAuthorCrudService authorCrudService)
    {
        this._authorRetrievalService = authorRetrievalService;
        this._authorCrudService = authorCrudService;
    }

    [HttpGet]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Index(int pageIndex, string? searchTerm)
    {
        PaginatedList<AllAuthorsViewModel> allAuthors = await this
            ._authorRetrievalService
            .GetAllAsync(pageIndex, 5, searchTerm);

        ViewBag.PrevDisabled = !allAuthors.HasPreviousPage;
        ViewBag.NextDisabled = !allAuthors.HasNextPage;
        ViewBag.ControllerName = nameof(Author);
        ViewBag.ActionName = nameof(Index);

        ViewBag.SearchTerm = searchTerm ?? string.Empty;

        return View(allAuthors);
    }

    [HttpGet]
    [Authorize(Roles = AdminRole)]
    public IActionResult Create()
    {
        CreateAuthorViewModel authorModel = this
            ._authorRetrievalService
            .GetAuthorForCreate();

        return View(authorModel);
    }

    [HttpPost]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Create(CreateAuthorViewModel authorModel)
    {
        if (!ModelState.IsValid)
        {
            return View(authorModel);
        }

        await this
            ._authorCrudService
            .CreateAuthorAsync(authorModel);

        TempData[Success] = AuthorHasBeenSuccessfullyCreated;

        return RedirectToAction(nameof(Index), nameof(Author));
    }

    [HttpGet]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Edit(Guid? id, int pageIndex, string? searchTerm)
    {
        if (id is not { } authorId || id == Guid.Empty)
        {
            return RedirectToAction(ErrorPageNotFoundAction, nameof(Error));
        }

        EditAuthorViewModel authorModel = await this
            ._authorRetrievalService
            .GetAuthorForEditAsync(authorId);
        
        ViewBag.PageIndex = pageIndex;
        ViewBag.SearchTerm = searchTerm!;

        return View(authorModel);
    }

    [HttpPost]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Edit(EditAuthorViewModel authorModel, int pageIndex, string? searchTerm)
    {
        if (!ModelState.IsValid)
        {
            return View(authorModel);
        }

        await this
            ._authorCrudService
            .EditAuthorAsync(authorModel);
        
        TempData[Success] = AuthorHasBeenSuccessfullyEdited;

        return RedirectToAction(nameof(Index), nameof(Author), new { pageIndex = pageIndex, searchTerm = searchTerm });
    }

    [HttpGet]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id is not { } authorId || id == Guid.Empty)
        {
            return RedirectToAction(ErrorPageNotFoundAction, nameof(Error));
        }

        DeleteAuthorViewModel authorModel = await this
            ._authorRetrievalService
            .GetAuthorForDeleteAsync(authorId);

        return this.View(authorModel);
    }

    [HttpPost]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Delete(DeleteAuthorViewModel authorModel)
    {
        await this
            ._authorCrudService
            .DeleteAuthorAsync(authorModel);

        TempData[Success] = AuthorHasBeenSuccessfullyDeleted;

        return RedirectToAction(nameof(Index), nameof(Author));
    }
}