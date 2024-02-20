namespace ReadersRealm.Areas.Admin.Controllers;

using Common;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Data.Contracts;
using ViewModels.Company;
using static Common.Constants.Constants.Areas;
using static Common.Constants.Constants.Company;
using static Common.Constants.Constants.Roles;
using static Common.Constants.Constants.Shared;

[Area(Admin)]
public class CompanyController : BaseController
{
    private readonly ICompanyService _companyService;

    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpGet]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Index(int pageIndex, string? searchTerm)
    {
        PaginatedList<AllCompaniesViewModel> allCompanies = await _companyService
            .GetAllAsync(pageIndex, 5, searchTerm);

        ViewBag.SearchTerm = searchTerm ?? string.Empty;

        return View(allCompanies);
    }

    [HttpGet]
    [Authorize(Roles = AdminRole)]
    public IActionResult Create()
    {
        CreateCompanyViewModel companyModel = _companyService
            .GetCompanyForCreate();

        return View(companyModel);
    }

    [HttpPost]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Create(CreateCompanyViewModel companyModel)
    {
        if (!ModelState.IsValid)
        {
            return View(companyModel);
        }

        await _companyService
            .CreateCompanyAsync(companyModel);

        TempData[Success] = CompanyHasBeenSuccessfullyCreated;

        return RedirectToAction(nameof(Index), nameof(Company));
    }

    [HttpGet]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Edit(Guid? id, int pageIndex, string? searchTerm)
    {
        if (id == null || id == Guid.Empty)
        {
            return NotFound();
        }

        EditCompanyViewModel companyModel = await _companyService
            .GetCompanyForEditAsync((Guid)id);

        ViewBag.PageIndex = pageIndex;
        ViewBag.SearchTerm = searchTerm ?? string.Empty;

        return View(companyModel);
    }

    [HttpPost]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Edit(EditCompanyViewModel companyModel, int pageIndex, string? searchTerm)
    {
        if (!ModelState.IsValid)
        {
            return View(companyModel);
        }

        await _companyService
            .EditCompanyAsync(companyModel);

        TempData[Success] = CompanyHasBeenSuccessfullyEdited;

        return RedirectToAction(nameof(Index), nameof(Company), new { pageIndex = pageIndex, searchTerm = searchTerm });
    }

    [HttpGet]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null || id == Guid.Empty)
        {
            return NotFound();
        }

        DeleteCompanyViewModel companyModel = await _companyService
            .GetCompanyForDeleteAsync((Guid)id);

        return View(companyModel);
    }

    [HttpPost]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Delete(DeleteCompanyViewModel companyModel)
    {
        await _companyService
            .DeleteCompanyAsync(companyModel);

        TempData[Success] = CompanyHasBeenSuccessfullyDeleted;

        return RedirectToAction(nameof(Index), nameof(Company));
    }
}