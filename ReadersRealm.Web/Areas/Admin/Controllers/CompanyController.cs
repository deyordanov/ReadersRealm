namespace ReadersRealm.Web.Areas.Admin.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Common;
using Data.Models;
using Services.Data.CompanyServices.Contracts;
using ViewModels.Company;
using static Common.Constants.Constants.AreasConstants;
using static Common.Constants.Constants.CompanyConstants;
using static Common.Constants.Constants.RolesConstants;
using static Common.Constants.Constants.SharedConstants;
using static Common.Constants.Constants.ErrorConstants;

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

        ViewBag.PrevDisabled = !allCompanies.HasPreviousPage;
        ViewBag.NextDisabled = !allCompanies.HasNextPage;
        ViewBag.ControllerName = nameof(Company);
        ViewBag.ActionName = nameof(Index);

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
        if (id is not { } companyId || id == Guid.Empty)
        {
            return RedirectToAction(ErrorPageNotFoundAction, nameof(Error));
        }

        EditCompanyViewModel companyModel = await this
            ._companyRetrievalService
            .GetCompanyForEditAsync(companyId);

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
        if (id is not { } companyId || id == Guid.Empty)
        {
            return RedirectToAction(ErrorPageNotFoundAction, nameof(Error));
        }

        DeleteCompanyViewModel companyModel = await this
            ._companyRetrievalService
            .GetCompanyForDeleteAsync(companyId);

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