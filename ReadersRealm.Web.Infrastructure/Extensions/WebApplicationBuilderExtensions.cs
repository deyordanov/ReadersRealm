namespace ReadersRealm.Web.Infrastructure.Extensions;

using System.Reflection;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Common;
using Common.Contracts;
using Common.Exceptions.Services;
using Common.Exceptions.User;
using Data;
using Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stripe;
using static Common.Constants.Constants.AzureKeyVaultConstants;
using static Common.Constants.Constants.RolesConstants;
using static Common.Constants.Constants.UserConstants;
using static Common.Constants.ExceptionMessages.ApplicationUserExceptionMessages;
using static Common.Constants.ExceptionMessages.ServiceExceptionMessages;

public static class WebApplicationBuilderExtensions
{
    public static async Task CreateAdminUserAsync(this IApplicationBuilder app)
    {
        using IServiceScope serviceScope = app.ApplicationServices.CreateScope();

        UserManager<ApplicationUser> userManager = serviceScope
            .ServiceProvider
            .GetRequiredService<UserManager<ApplicationUser>>();

        ApplicationUser? existentUser = await  
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
                        UserName = AdminUserEmail,
                        EmailConfirmed = AdminUserEmailConfirmed,
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
                ApplicationUser? adminUser = await userManager
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

        RoleManager<IdentityRole<Guid>> roleManager = serviceScope
                .ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole<Guid>>>();

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
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }
    }

    public static void AddApplicationServices(
        this IServiceCollection services, 
        Type getAssemblyServiceType)
    {
        Assembly? assembly = Assembly.GetAssembly(getAssemblyServiceType);

        if (assembly == null)
        {
            throw new ServiceTypeNotFoundException(string.Format(ServiceTypeNotFoundExceptionMessage,
                getAssemblyServiceType.Name));
        }

        ICollection<Type> serviceTypes = assembly
            .GetTypes()
            .Where(t => t.Name.EndsWith("Service") &&
                        !t.IsInterface)
            .ToList();

        foreach (Type serviceType in serviceTypes)
        {
            Type? interfaceType = serviceType.GetInterface($"I{serviceType.Name}");

            if (interfaceType == null)
            {
                throw new ServiceInterfaceNotFound(
                    string.Format(ServiceInterfaceNotFoundExceptionMessage,
                        serviceType.Name));
            }

            services.AddScoped(interfaceType, serviceType);
        }
    }

    public static void AddApplicationIdentity(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = configuration
                    .GetValue<bool>("Identity:RequireConfirmedAccount");

                options.Password.RequireDigit = configuration
                    .GetValue<bool>("Identity:RequireDigit");

                options.Password.RequireNonAlphanumeric = configuration
                    .GetValue<bool>("Identity:RequireNonAlphanumeric");

                options.Password.RequireLowercase = configuration
                    .GetValue<bool>("Identity:RequireLowercase");

                options.Password.RequireUppercase = configuration
                    .GetValue<bool>("Identity:RequireUppercase");
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ReadersRealmDbContext>()
            .AddDefaultTokenProviders();
    }

    public static async Task SetConfigurationSettings(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        Uri keyVaultUri = new Uri(configuration[AzureKeyVaultUri]!);
        SecretClient client = new SecretClient(keyVaultUri, new DefaultAzureCredential());
        KeyVaultSecret stripeSecretKey = await client.GetSecretAsync(AzureKeyVaultStripeSecretKey);
        KeyVaultSecret stripePublishableKey = await client.GetSecretAsync(AzureKeyVaultStripePublishableKey);

        StripeConfiguration.ApiKey = stripeSecretKey.Value;

        KeyVaultSecret sendGridSecretKey = await client.GetSecretAsync(AzureKeyVaultSendGridSecretKey);

        ISendGridSettings sendGridSettings = new SendGridSettings()
        {
            SecretKey = sendGridSecretKey.Value,
        };

        services.AddSingleton(sendGridSettings);
    }
}