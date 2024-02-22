using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using ReadersRealm.Common;
using ReadersRealm.Data;
using ReadersRealm.Data.Repositories;
using ReadersRealm.Data.Repositories.Contracts;
using ReadersRealm.Services.Data.ApplicationUserServices.Contracts;
using ReadersRealm.Web.Infrastructure.Extensions;
using Stripe;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddApplicationServices(typeof(IApplicationUserRetrievalService));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.AddHttpContextAccessor();

//Adding the db context to the container.
builder.Services.AddDbContext<ReadersRealmDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddApplicationIdentity(builder.Configuration);

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
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
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

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