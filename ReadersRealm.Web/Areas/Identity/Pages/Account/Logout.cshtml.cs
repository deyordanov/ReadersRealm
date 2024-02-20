// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

namespace ReadersRealm.Web.Areas.Identity.Pages.Account;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Contracts;
using static Common.Constants.Constants.SessionKeys;

public class LogoutModel : PageModel
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ILogger<LogoutModel> _logger;
    private readonly IShoppingCartService _shoppingCartService;

    public LogoutModel(
        SignInManager<IdentityUser> signInManager, 
        ILogger<LogoutModel> logger, 
        IShoppingCartService shoppingCartService)
    {
        _signInManager = signInManager;
        _logger = logger;
        _shoppingCartService = shoppingCartService;
    }

    public async Task<IActionResult> OnPost(string returnUrl = null)
    {
        HttpContext.Session.Remove(ShoppingCartSessionKey);

        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out.");
        if (returnUrl != null)
        {
            return LocalRedirect(returnUrl);
        }
        else
        {
            // This needs to be a redirect so that the browser performs a new
            // request and the identity for the user gets updated.
            return RedirectToPage();
        }
    }
}