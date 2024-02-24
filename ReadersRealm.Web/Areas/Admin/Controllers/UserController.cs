namespace ReadersRealm.Web.Areas.Admin.Controllers;

using Common;
using Microsoft.AspNetCore.Mvc;
using Services.Data.ApplicationUserServices.Contracts;
using ViewModels.ApplicationUser;
using static Common.Constants.Constants.Areas;

[Area(Admin)]
public class UserController : BaseController
{
    private readonly IApplicationUserRetrievalService _applicationUserRetrievalService;

    public UserController(IApplicationUserRetrievalService applicationUserRetrievalService)
    {
        this._applicationUserRetrievalService = applicationUserRetrievalService;
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
}