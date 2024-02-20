namespace ReadersRealm.Web.Infrastructure.Extensions;

using Common.Exceptions.User;
using Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using static Common.Constants.Constants.Roles;
using static Common.Constants.Constants.User;
using static Common.Constants.ExceptionMessages.ApplicationUserExceptionMessages;

public static class WebApplicationBuilderExtensions
{
    public static async Task CreateAdminUserAsync(this IApplicationBuilder app)
    {
        using IServiceScope serviceScope = app.ApplicationServices.CreateScope();

        UserManager<IdentityUser> userManager = serviceScope
            .ServiceProvider
            .GetRequiredService<UserManager<IdentityUser>>();

        IdentityUser? existentUser = await
            userManager
                .FindByEmailAsync(AdminUserEmail);

        if (existentUser == null)
        {
            IdentityResult result = await userManager
                    .CreateAsync(new ApplicationUser()
                    {
                        FirstName = AdminUserFirstName,
                        LastName = AdminUserLastName,
                        City = AdminUserCity,
                        Email = AdminUserEmail,
                        PhoneNumber = AdminUserPhoneNumber,
                        PostalCode = AdminUserPostalCode,
                        State = AdminUserState,
                        StreetAddress = AdminUserStreetAddress,
                        UserName = AdminUserEmail
                    }, AdminUserPassword);

            if (!result.Succeeded)
            {
                string errors = string.Join(", ", result
                    .Errors
                    .Select(e => $"{e.Code}: {e.Description}"));

                throw new UserCreationFailedException(string.Format(UserCreationFailedExceptionMessage, errors));
            }
            else
            {
                IdentityUser? adminUser = await userManager
                    .FindByEmailAsync(AdminUserEmail);

                if (adminUser == null)
                {
                    throw new UserNotFoundException();
                }

                await userManager.AddToRoleAsync(adminUser, AdminRole);
            }
        }
    }

    public static async Task CreateRolesAsync(this IApplicationBuilder app)
    {
        IServiceScope serviceScope = app.ApplicationServices.CreateScope();

        RoleManager<IdentityRole> roleManager = serviceScope
                .ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

        IList<string> roles = new List<string>()
        {
            AdminRole,
            CompanyRole,
            EmployeeRole,
            CustomerRole,
        };

        foreach (string role in roles)
        {
            bool roleExists = await roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}