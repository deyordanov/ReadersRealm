using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ReadersRealm.Common;
using ReadersRealm.Data;
using ReadersRealm.Data.Repositories;
using ReadersRealm.Data.Repositories.Contracts;
using ReadersRealm.Services.Data.ApplicationUserServices.Contracts;
using ReadersRealm.Web.Infrastructure.Extensions;
using ReadersRealm.Web.Infrastructure.Middlewares;
using ReadersRealm.Web.Infrastructure.ModelBinders;
using ReadersRealm.Web.Infrastructure.Settings;
using ReadersRealm.Web.Infrastructure.Settings.Contracts;
using static ReadersRealm.Common.Constants.Constants.ConfigurationConstants;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
await builder.Services.SetConfigurationSettings(builder.Configuration);

builder.Services.AddControllersWithViews()
    .AddMvcOptions(options =>
    {
        options
            .ModelBinderProviders
            .Insert(0, new DecimalModelBinderProvider());
    });
builder.Services.AddRazorPages();

builder.Services.AddAntiforgery(options =>
{
    options.FormFieldName = "__RequestVerificationToken";
    options.HeaderName = "X-CSRF-VERIFICATION-TOKEN";
    options.SuppressXFrameOptionsHeader = false;
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.AddHttpContextAccessor();

//Adding the db context to the container.
builder.Services.AddDbContext<ReadersRealmDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddApplicationIdentity(builder.Configuration);

builder.Services.AddApplicationServices(typeof(IApplicationUserRetrievalService));

//Configure settings
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection(MongoDbSettingsKey));

builder.Services.AddSingleton<IMongoDbSettings>(sp => 
    sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Customer/User/Login";
    options.LogoutPath = "/Customer/User/Logout";
    options.AccessDeniedPath = "/Customer/User/Register";
});

/*
    Configure the application to use an in-memory cache - this cache (IDistributedCache) is local to each server, so other servers will not be able to access the data stored in the cache. For other larger, more complex applications, which use multiple servers we can use Redis
 */
builder.Services.AddDistributedMemoryCache();

//Configure the session middleware
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(120);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // app.UseExceptionHandler("/Home/Error");
    // app.UseStatusCodePagesWithRedirects("/Customer/Error/{0}"); -> the {0} will be replaced with the error status code.
    app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//Add the session middleware to the pipeline.
app.UseSession();

await app.CreateRolesAsync();

await app.CreateAdminUserAsync();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();