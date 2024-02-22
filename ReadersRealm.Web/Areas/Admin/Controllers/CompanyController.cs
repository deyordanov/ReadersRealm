namespace ReadersRealm.Web.Areas.Admin.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Common;
using Data.Models;
using Services.Data.CompanyServices.Contracts;
using ViewModels.Company;
using static Common.Constants.Constants.Areas;
using static Common.Constants.Constants.Company;
using static Common.Constants.Constants.Roles;
using static Common.Constants.Constants.Shared;

[Area(Admin)]
public class CompanyController : BaseController
{
    private readonly ICompanyCrudService _companyCrudService;
    private readonly ICompanyRetrievalService _companyRetrievalService;

    public CompanyController(
        ICompanyCrudService companyCrudService, 
        ICompanyRetrievalService companyRetrievalService)
    {
        this._companyCrudService = companyCrudService;
        this._companyRetrievalService = companyRetrievalService;
    }

    [HttpGet]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Index(int pageIndex, string? searchTerm)
    {
        PaginatedList<AllCompaniesViewModel> allCompanies = await this
            ._companyRetrievalService
            .GetAllAsync(pageIndex, 5, searchTerm);

        ViewBag.SearchTerm = searchTerm ?? string.Empty;

        return View(allCompanies);
    }

    [HttpGet]
    [Authorize(Roles = AdminRole)]
    public IActionResult Create()
    {
        CreateCompanyViewModel companyModel = this
            ._companyRetrievalService
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

        await this
            ._companyCrudService
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

        EditCompanyViewModel companyModel = await this
            ._companyRetrievalService
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

        await this
            ._companyCrudService
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

        DeleteCompanyViewModel companyModel = await this
            ._companyRetrievalService
            .GetCompanyForDeleteAsync((Guid)id);

        return View(companyModel);
    }

    [HttpPost]
    [Authorize(Roles = AdminRole)]
    public async Task<IActionResult> Delete(DeleteCompanyViewModel companyModel)
    {
        await this
            ._companyCrudService
            .DeleteCompanyAsync(companyModel);

        TempData[Success] = CompanyHasBeenSuccessfullyDeleted;

        return RedirectToAction(nameof(Index), nameof(Company));
    }
}