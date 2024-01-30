using Microsoft.AspNetCore.Mvc;

namespace ReadersRealm.Controllers;

using Services.Contracts;
using ViewModels.Category;

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
}