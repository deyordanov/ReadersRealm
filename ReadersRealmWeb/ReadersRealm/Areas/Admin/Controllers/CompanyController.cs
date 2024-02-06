using Microsoft.AspNetCore.Mvc;

namespace ReadersRealm.Areas.Admin.Controllers;

using Common;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Services.Contracts;
using ViewModels.Company;
using static Common.Constants.Constants.Company;
using static Common.Constants.Constants.Roles;
using static Common.Constants.Constants.Shared;

[Area("Admin")]
[Authorize(Roles = AdminRole)]
public class CompanyController : Controller
{
    private readonly ICompanyService companyService;

    public CompanyController(ICompanyService companyService)
    {
        this.companyService = companyService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int pageIndex, string? searchTerm)
    {
        PaginatedList<AllCompaniesViewModel> allCompanies = await this
            .companyService
            .GetAllAsync(pageIndex, 5, searchTerm);

        ViewBag.SearchTerm = searchTerm ?? "";

        return View(allCompanies);
    }

    [HttpGet]
    public IActionResult Create()
    {
        CreateCompanyViewModel companyModel = this
            .companyService
            .GetCompanyForCreate();

        return View(companyModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCompanyViewModel companyModel)
    {
        if (!ModelState.IsValid)
        {
            return View(companyModel);
        }

        await this
            .companyService
            .CreateCompanyAsync(companyModel);

        TempData[Success] = CompanyHasBeenSuccessfullyCreated;

        return RedirectToAction(nameof(Index), nameof(Company));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid? id, int pageIndex, string? searchTerm)
    {
        if (id == null || id == Guid.Empty)
        {
            return NotFound();
        }

        EditCompanyViewModel companyModel = await this
            .companyService
            .GetCompanyForEditAsync((Guid)id);

        ViewBag.PageIndex = pageIndex;
        ViewBag.SearchTerm = searchTerm ?? "";

        return View(companyModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditCompanyViewModel companyModel, int pageIndex, string? searchTerm)
    {
        if (!ModelState.IsValid)
        {
            return View(companyModel);
        }

        await this
            .companyService
            .EditCompanyAsync(companyModel);

        TempData[Success] = CompanyHasBeenSuccessfullyEdited;

        return RedirectToAction(nameof(Index), nameof(Company), new { pageIndex = pageIndex, searchTerm = searchTerm });
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null || id == Guid.Empty)
        {
            return NotFound();
        }

        DeleteCompanyViewModel companyModel = await this
            .companyService
            .GetCompanyForDeleteAsync((Guid)id);

        return View(companyModel);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(DeleteCompanyViewModel companyModel)
    {
        await this
            .companyService
            .DeleteCompanyAsync(companyModel);

        TempData[Success] = CompanyHasBeenSuccessfullyDeleted;

        return RedirectToAction(nameof(Index), nameof(Company));
    }
}