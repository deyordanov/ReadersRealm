namespace ReadersRealm.Web.Areas.Admin.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Data.Models;
using Services.Data.CategoryServices.Contracts;
using ViewModels.Category;
using static Common.Constants.Constants.Areas;
using static Common.Constants.Constants.Category;
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
        IEnumerable<AllCategoriesViewModel> allCategories = await
            _categoryRetrievalService
            .GetAllAsync();

        return View(allCategories);
    }

    [HttpGet]
    [Authorize(Roles = AdminRole)]
    public IActionResult Create()
    {
        CreateCategoryViewModel categoryModel = _categoryRetrievalService
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
        if (id == null || id <= 0)
        {
            return NotFound();
        }

        EditCategoryViewModel categoryModel = await _categoryRetrievalService
            .GetCategoryForEditAsync((int)id);

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

        await
            _categoryCrudService
            .EditCategoryAsync(categoryModel);

        TempData[Success] = CategoryHasBeenSuccessfullyEdited;

        return RedirectToAction(nameof(Index), nameof(Category));
    }

    [HttpGet]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || id <= 0)
        {
            return NotFound();
        }

        DeleteCategoryViewModel categoryModel = await _categoryRetrievalService
            .GetCategoryForDeleteAsync((int)id);

        return View(categoryModel);
    }

    [HttpPost]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Delete(DeleteCategoryViewModel categoryModel)
    {
        await
            _categoryCrudService
            .DeleteCategoryAsync(categoryModel);

        TempData[Success] = CategoryHasBeenSuccessfullyDeleted;

        return RedirectToAction(nameof(Index), nameof(Category));
    }
}