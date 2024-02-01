namespace ReadersRealm.Areas.Admin.Controllers;

using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using ViewModels.Category;
using static Common.Constants.Category;
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
        return View();
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

        TempData["Success"] = CategoryHasBeenSuccessfullyCreated;

        return RedirectToAction("Index", "Category");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        Category? category = await 
            categoryService
            .GetCategoryByIdAsync((int)id);

        if (category == null)
        {
            return NotFound();
        }

        EditCategoryViewModel categoryModel = new EditCategoryViewModel()
        {
            Id = category.Id,
            Name = category.Name,
            DisplayOrder = category.DisplayOrder,
        };

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

        TempData["Success"] = CategoryHasBeenSuccessfullyEdited;

        return RedirectToAction("Index", "Category");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || id <= 0)
        {
            return NotFound();
        }

        Category? category = await 
            categoryService
            .GetCategoryByIdAsync((int)id);

        if (category == null)
        {
            return NotFound();
        }

        DeleteCategoryViewModel categoryModel = new DeleteCategoryViewModel()
        {
            Id = category.Id,
            Name = category.Name,
            DisplayOrder = category.DisplayOrder,
        };

        return View(categoryModel);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(DeleteCategoryViewModel categoryModel)
    {
        await 
            categoryService
            .DeleteCategoryAsync(categoryModel);

        TempData["Success"] = CategoryHasBeenSuccessfullyDeleted;

        return RedirectToAction("Index", "Category");
    }
}