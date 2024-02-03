namespace ReadersRealm.Areas.Admin.Controllers;

using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using ViewModels.Category;
using static Common.Constants.Category;
using static Common.Constants.Shared;
using static Common.ValidationMessages.Category;

[Area("Admin")]
public class CategoryController : Controller
{
    private ICategoryService categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        this.categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        IEnumerable<AllCategoriesViewModel> allCategories = await 
            categoryService
            .GetAllAsync();

        return View(allCategories);
    }

    [HttpGet]
    public IActionResult Create()
    {
        CreateCategoryViewModel categoryModel = this
            .categoryService
            .GetCategoryForCreate();

        return View(categoryModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryViewModel categoryModel)
    {
        if (categoryModel.Name.ToLower() == categoryModel.DisplayOrder.ToString())
        {
            ModelState.AddModelError("Name", MatchingNameAndDisplayOrderMessage);
        }

        if (!ModelState.IsValid)
        {
            return View(categoryModel);
        }

        await 
            categoryService
            .CreateCategoryAsync(categoryModel);

        TempData[Success] = CategoryHasBeenSuccessfullyCreated;

        return RedirectToAction(nameof(Index), nameof(Category));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || id <= 0)
        {
            return NotFound();
        }

        EditCategoryViewModel categoryModel = await this
            .categoryService
            .GetCategoryForEditAsync((int)id);

        return View(categoryModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditCategoryViewModel categoryModel)
    {
        if (categoryModel.Name.ToLower() == categoryModel.DisplayOrder.ToString())
        {
            ModelState.AddModelError("Name", MatchingNameAndDisplayOrderMessage);
        }

        if (!ModelState.IsValid)
        {
            return View(categoryModel);
        }

        await 
            categoryService
            .EditCategoryAsync(categoryModel);

        TempData[Success] = CategoryHasBeenSuccessfullyEdited;

        return RedirectToAction(nameof(Index), nameof(Category));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || id <= 0)
        {
            return NotFound();
        }

        DeleteCategoryViewModel categoryModel = await this
            .categoryService
            .GetCategoryForDeleteAsync((int)id);

        return View(categoryModel);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(DeleteCategoryViewModel categoryModel)
    {
        await 
            categoryService
            .DeleteCategoryAsync(categoryModel);

        TempData[Success] = CategoryHasBeenSuccessfullyDeleted;

        return RedirectToAction(nameof(Index), nameof(Category));
    }
}