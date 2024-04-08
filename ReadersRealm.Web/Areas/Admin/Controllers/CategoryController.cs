namespace ReadersRealm.Web.Areas.Admin.Controllers;

using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Data.CategoryServices.Contracts;
using ViewModels.Category;
using static Common.Constants.Constants.AreasConstants;
using static Common.Constants.Constants.CategoryConstants;
using static Common.Constants.Constants.ErrorConstants;
using static Common.Constants.Constants.RolesConstants;
using static Common.Constants.Constants.SharedConstants;
using static Common.Constants.ValidationMessageConstants.CategoryValidationMessages;


[Area(Admin)]
public class CategoryController(
    ICategoryCrudService categoryCrudService,
    ICategoryRetrievalService categoryRetrievalService)
    : BaseController
{
    [HttpGet]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Index(int pageIndex, string? searchTerm)
    {
        IEnumerable<AllCategoriesViewModel> allCategories = await categoryRetrievalService
            .GetAllAsync();

        return View(allCategories);
    }

    [HttpGet]
    [Authorize(Roles = AdminRole)]
    public IActionResult Create()
    {
        CreateCategoryViewModel categoryModel = categoryRetrievalService
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
            categoryCrudService
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

        EditCategoryViewModel categoryModel = await categoryRetrievalService
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

        await categoryCrudService
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

        DeleteCategoryViewModel categoryModel = await categoryRetrievalService
            .GetCategoryForDeleteAsync(bookId);

        return View(categoryModel);
    }

    [HttpPost]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Delete(DeleteCategoryViewModel categoryModel)
    {
        await categoryCrudService
            .DeleteCategoryAsync(categoryModel);

        TempData[Success] = CategoryHasBeenSuccessfullyDeleted;

        return RedirectToAction(nameof(Index), nameof(Category));
    }
}