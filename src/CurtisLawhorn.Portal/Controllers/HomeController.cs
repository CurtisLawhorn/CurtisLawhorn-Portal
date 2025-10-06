using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CurtisLawhorn.Portal.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.WebUtilities;

namespace CurtisLawhorn.Portal.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        return View();
    }
    
    [AllowAnonymous]
    public IActionResult Privacy()
    {
        return View();
    }
    
    public IActionResult Secure()
    {
        return View();
    }
    
    public new async Task<IActionResult> SignOut()
    {
        // Get the saved ID token securely
        var idToken = await HttpContext.GetTokenAsync("id_token");

        // Sign out local cookies first
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        // Build the logout URL server-side
        var request = HttpContext.Request;
        var rootUrl = $"{request.Scheme}://{request.Host}/";

        var logoutUrl = QueryHelpers.AddQueryString(
            "https://us-east-2dhhagrvr2.auth.us-east-2.amazoncognito.com/logout",
            new Dictionary<string, string>
            {
                { "client_id", "1805e6pjem9rd2nhfce1jbuva5" },
                { "id_token_hint", idToken },
                { "logout_uri", rootUrl }
            });

        // Redirect to Cognito logout
        return Redirect(logoutUrl);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
}