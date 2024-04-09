namespace ReadersRealm.Web.Areas.Admin.Controllers;

using Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Data.ApplicationUserServices.Contracts;
using Services.Data.CompanyServices.Contracts;
using ViewModels.ApplicationUser;
using static Common.Constants.Constants.AreasConstants;
using static Common.Constants.Constants.ErrorConstants;
using static Common.Constants.Constants.SharedConstants;
using static Common.Constants.ValidationMessageConstants.UserValidationMessages;
using static Common.Constants.ValidationMessageConstants.CompanyValidationMessages;

[Area(Admin)]
public class UserController(
    IApplicationUserRetrievalService applicationUserRetrievalService,
    IApplicationUserCrudService applicationUserCrudService,
    ICompanyRetrievalService companyRetrievalService,
    RoleManager<IdentityRole<Guid>> roleManager)
    : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Index(int pageIndex, string? searchTerm)
    {
        PaginatedList<AllApplicationUsersViewModel> allApplicationUsers = await applicationUserRetrievalService
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
        RolesApplicationUserViewModel applicationUserModel = await applicationUserRetrievalService
            .GetApplicationUserForRolesManagementAsync(applicationUserId);

        return View(applicationUserModel);
    }

    [HttpPost]
    public async Task<IActionResult> ManageRoles(int pageIndex, string? searchTerm, RolesApplicationUserViewModel applicationUserModel)
    {
        if (!ModelState.IsValid)
        {
            applicationUserModel = await applicationUserRetrievalService
                .GetApplicationUserForRolesManagementAsync(applicationUserModel.Id);

            return View(applicationUserModel);
        }

        IList<string> allRoles = (await roleManager
            .Roles
            .Select(r => r.Name)
            .ToListAsync())!;

        bool areRolesInvalid = allRoles.Except(applicationUserModel.NewRoles).Count() !=
                             allRoles.Count - applicationUserModel.NewRoles.Count;
        if (areRolesInvalid)
        {
            ModelState.AddModelError(nameof(applicationUserModel.NewRoles), RoleDoesNotExistMessage);

            applicationUserModel = await applicationUserRetrievalService
                .GetApplicationUserForRolesManagementAsync(applicationUserModel.Id);

            return View(applicationUserModel);
        }

        bool companyExists = applicationUserModel.CompanyId == null ||
                             await companyRetrievalService
                                 .CompanyExistsAsync((Guid)applicationUserModel.CompanyId);

        if (!companyExists)
        {
            ModelState.AddModelError(nameof(applicationUserModel.CompanyId), CompanyDoesNotExistMessage);

            applicationUserModel = await applicationUserRetrievalService
                .GetApplicationUserForRolesManagementAsync(applicationUserModel.Id);

            return View(applicationUserModel);
        }

        await applicationUserCrudService
            .UpdateApplicationUserRolesAsync(applicationUserModel);
    
        return RedirectToAction(nameof(Index), nameof(User));
    }

    [HttpGet]
    public async Task<IActionResult> Lock(Guid? id)
    {
        if (id is not { } applicationUserId || id == Guid.Empty)
        {
            return RedirectToAction(ErrorPageNotFoundAction, nameof(Error));
        }

        await applicationUserCrudService
            .UpdateApplicationUserLockoutAsync(applicationUserId, true);

        return RedirectToAction(nameof(Index), nameof(User));
    }

    [HttpGet]
    public async Task<IActionResult> Unlock(Guid? id)
    {
        if (id is not { } applicationUserId || id == Guid.Empty)
        {
            return RedirectToAction(ErrorPageNotFoundAction, nameof(Error));
        }

        await applicationUserCrudService
            .UpdateApplicationUserLockoutAsync(applicationUserId, false);

        return RedirectToAction(nameof(Index), nameof(User));
    }
}