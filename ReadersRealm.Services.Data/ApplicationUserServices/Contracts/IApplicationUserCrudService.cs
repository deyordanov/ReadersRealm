﻿namespace ReadersRealm.Services.Data.ApplicationUserServices.Contracts;

using Web.ViewModels.ApplicationUser;

public interface IApplicationUserCrudService
{
    Task UpdateApplicationUserAsync(OrderApplicationUserViewModel applicationUserModel);
}