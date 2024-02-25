namespace ReadersRealm.Web.Areas.Admin.Controllers;

using Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Data.ApplicationUserServices.Contracts;
using ViewModels.ApplicationUser;
using static Common.Constants.Constants.Areas;

[Area(Admin)]
public class UserController : BaseController
{
    private readonly IApplicationUserRetrievalService _applicationUserRetrievalService;
    private readonly IApplicationUserCrudService _applicationUserCrudService;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public UserController(
        IApplicationUserRetrievalService applicationUserRetrievalService, 
        RoleManager<IdentityRole<Guid>> roleManager, 
        IApplicationUserCrudService applicationUserCrudService)
    {
        this._applicationUserRetrievalService = applicationUserRetrievalService;
        this._roleManager = roleManager;
        this._applicationUserCrudService = applicationUserCrudService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int pageIndex, string? searchTerm)
    {
        PaginatedList<AllApplicationUsersViewModel> allApplicationUsers = await this
            ._applicationUserRetrievalService
            .GetAllAsync(pageIndex, 6, searchTerm);

        ViewBag.PrevDisabled = !allApplicationUsers.HasPreviousPage;
        ViewBag.NextDisabled = !allApplicationUsers.HasNextPage;
        ViewBag.ControllerName = nameof(User);
        ViewBag.ActionName = nameof(Index);

        ViewBag.SearchTerm = searchTerm ?? "";

        return View(allApplicationUsers);
    }

    [HttpGet]
    public async Task<IActionResult> ManageRoles(int pageIndex, string? searchTerm, Guid applicationUserId)
    {
        RolesApplicationUserViewModel applicationUserModel = await this
            ._applicationUserRetrievalService
            .GetApplicationUserForRolesManagementAsync(applicationUserId);

        return View(applicationUserModel);
    }

    [HttpPost]
    public async Task<IActionResult> ManageRoles(int pageIndex, string? searchTerm, RolesApplicationUserViewModel applicationUserModel)
    {
        if (!ModelState.IsValid)
        {
            applicationUserModel = await this
                ._applicationUserRetrievalService
                .GetApplicationUserForRolesManagementAsync(applicationUserModel.Id);

            return View(applicationUserModel);
        }

        IList<string> allRoles = (await this
            ._roleManager
            .Roles
            .Select(r => r.Name)
            .ToListAsync())!;

        bool areRolesValid = allRoles.Except(applicationUserModel.NewRoles).Count() !=
                             allRoles.Count - applicationUserModel.NewRoles.Count;
        if (areRolesValid)
        {
            ModelState.AddModelError(nameof(applicationUserModel.NewRoles), "Nope.");

            applicationUserModel = await this
                ._applicationUserRetrievalService
                .GetApplicationUserForRolesManagementAsync(applicationUserModel.Id);

            return View(applicationUserModel);
        }

        await this
            ._applicationUserCrudService
            .UpdateApplicationUserRolesAsync(applicationUserModel);
    
        return RedirectToAction(nameof(Index), nameof(User));
    }

    [HttpGet]
    public async Task<IActionResult> Lock(Guid applicationUserId)
    {
        await this
            ._applicationUserCrudService
            .UpdateApplicationUserLockoutAsync(applicationUserId, true);

        return RedirectToAction(nameof(Index), nameof(User));
    }

    [HttpGet]
    public async Task<IActionResult> Unlock(Guid applicationUserId)
    {
        await this
            ._applicationUserCrudService
            .UpdateApplicationUserLockoutAsync(applicationUserId, false);

        return RedirectToAction(nameof(Index), nameof(User));
    }
}