using Microsoft.AspNetCore.Mvc;

namespace ReadersRealm.Controllers;

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
}