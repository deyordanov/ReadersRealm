namespace ReadersRealm.Web.Areas.Admin.Controllers;

using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Data.CategoryServices.Contracts;
using ViewModels.Category;
using static Common.Constants.Constants.Areas;
using static Common.Constants.Constants.Category;
using static Common.Constants.Constants.Error;
using static Common.Constants.Constants.Roles;
using static Common.Constants.Constants.Shared;
using static Common.Constants.ValidationMessageConstants.Category;


[Area(Admin)]
public class CategoryController : BaseController
{
    private readonly ICategoryCrudService _categoryCrudService;
    private readonly ICategoryRetrievalService _categoryRetrievalService;
    public CategoryController(
        ICategoryCrudService categoryCrudService, 
        ICategoryRetrievalService categoryRetrievalService)
    {
        this._categoryCrudService = categoryCrudService;
        this._categoryRetrievalService = categoryRetrievalService;
    }

    [HttpGet]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Index(int pageIndex, string? searchTerm)
    {
        IEnumerable<AllCategoriesViewModel> allCategories = await this
            ._categoryRetrievalService
            .GetAllAsync();

        return View(allCategories);
    }

    [HttpGet]
    [Authorize(Roles = AdminRole)]
    public IActionResult Create()
    {
        CreateCategoryViewModel categoryModel = this
            ._categoryRetrievalService
            .GetCategoryForCreate();

        return View(categoryModel);
    }

    [HttpPost]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Create(CreateCategoryViewModel categoryModel)
    {
        if (categoryModel.Name.ToLower() == categoryModel.DisplayOrder.ToString())
        {
            ModelState.AddModelError(Name, MatchingNameAndDisplayOrderMessage);
        }

        if (!ModelState.IsValid)
        {
            return View(categoryModel);
        }

        await
            _categoryCrudService
            .CreateCategoryAsync(categoryModel);

        TempData[Success] = CategoryHasBeenSuccessfullyCreated;

        return RedirectToAction(nameof(Index), nameof(Category));
    }

    [HttpGet]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is not { } categoryId || id <= 0)
        {
            return RedirectToAction(ErrorPageNotFoundAction, nameof(Error));
        }

        EditCategoryViewModel categoryModel = await this
            ._categoryRetrievalService
            .GetCategoryForEditAsync(categoryId);

        return View(categoryModel);
    }

    [HttpPost]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Edit(EditCategoryViewModel categoryModel)
    {
        if (categoryModel.Name.ToLower() == categoryModel.DisplayOrder.ToString())
        {
            ModelState.AddModelError(Name, MatchingNameAndDisplayOrderMessage);
        }

        if (!ModelState.IsValid)
        {
            return View(categoryModel);
        }

        await this
            ._categoryCrudService
            .EditCategoryAsync(categoryModel);

        TempData[Success] = CategoryHasBeenSuccessfullyEdited;

        return RedirectToAction(nameof(Index), nameof(Category));
    }

    [HttpGet]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id is not { } bookId || id <= 0)
        {
            return RedirectToAction(ErrorPageNotFoundAction, nameof(Error));
        }

        DeleteCategoryViewModel categoryModel = await this
            ._categoryRetrievalService
            .GetCategoryForDeleteAsync(bookId);

        return View(categoryModel);
    }

    [HttpPost]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Delete(DeleteCategoryViewModel categoryModel)
    {
        await this
            ._categoryCrudService
            .DeleteCategoryAsync(categoryModel);

        TempData[Success] = CategoryHasBeenSuccessfullyDeleted;

        return RedirectToAction(nameof(Index), nameof(Category));
    }
}