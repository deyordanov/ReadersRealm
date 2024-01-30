using Microsoft.AspNetCore.Mvc;

namespace ReadersRealm.Controllers;

using DataModels;
using Services.Contracts;
using ViewModels.Category;
using static ReadersRealm.Common.ValidationMessages.Category;

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
        IEnumerable<AllCategoriesViewModel> allCategories = await this
            .categoryService
            .GetAllAsync();

        return View(allCategories);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateCategoryViewModel());
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

        await this
            .categoryService
            .CreateCategoryAsync(categoryModel);

        return RedirectToAction("Index", "Category");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        Category? category = await this
            .categoryService
            .GetCategoryById(id);

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

        await this
            .categoryService
            .EditCategory(categoryModel);

        return RedirectToAction("Index", "Category");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        Category? category = await this
            .categoryService
            .GetCategoryById(id);

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
        await this
            .categoryService
            .DeleteCategory(categoryModel);

        return RedirectToAction("Index", "Category");
    }
}