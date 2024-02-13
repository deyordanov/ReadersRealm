namespace ReadersRealm.Areas.Admin.Controllers;

using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using ViewModels.Category;
using static Common.Constants.Constants.Areas;
using static Common.Constants.Constants.Category;
using static Common.Constants.Constants.Roles;
using static Common.Constants.Constants.Shared;
using static Common.Constants.ValidationMessageConstants.Category;


[Area(Admin)]
public class CategoryController : BaseController
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        this._categoryService = categoryService;
    }

    [HttpGet]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Index(int pageIndex, string? searchTerm)
    {
        IEnumerable<AllCategoriesViewModel> allCategories = await 
            _categoryService
            .GetAllAsync();

        return View(allCategories);
    }

    [HttpGet]
    [Authorize(Roles = AdminRole)]
    public IActionResult Create()
    {
        CreateCategoryViewModel categoryModel = this
            ._categoryService
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
            _categoryService
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

        EditCategoryViewModel categoryModel = await this
            ._categoryService
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
            _categoryService
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

        DeleteCategoryViewModel categoryModel = await this
            ._categoryService
            .GetCategoryForDeleteAsync((int)id);

        return View(categoryModel);
    }

    [HttpPost]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Delete(DeleteCategoryViewModel categoryModel)
    {
        await 
            _categoryService
            .DeleteCategoryAsync(categoryModel);

        TempData[Success] = CategoryHasBeenSuccessfullyDeleted;

        return RedirectToAction(nameof(Index), nameof(Category));
    }
}