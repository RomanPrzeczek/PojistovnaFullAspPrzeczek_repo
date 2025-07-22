using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using PojistovnaFullAspPrzeczek.Models;
using PojistovnaFullAspPrzeczek.Interfaces;

namespace PojistovnaFullAspPrzeczek.Controllers;

public class HomeController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IInsuredPersonService _insuredPersonService; public HomeController(IInsuredPersonService insuredPersonService, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _insuredPersonService = insuredPersonService;
    }

    public async Task<IActionResult> Index()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    return RedirectToAction("Index", "InsuredPersons"); // přehled všech
                }

                var insuredPerson = await _insuredPersonService.GetByEmailAsync(user.Email);

                //await _context.InsuredPerson
                //.FirstOrDefaultAsync(p => p.Email == user.Email);

                if (insuredPerson != null)
                {
                    return RedirectToAction("Details", "InsuredPersons", new { id = insuredPerson.IdInsuredPerson });
                }
            }
        }

        // Nepřihlášený nebo bez vazby → zobraz login nebo welcome
        return View();
    }


    public IActionResult Events()
    {
        return View();
    }

    public IActionResult AboutApp()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult ChangeLanguage(string culture, string returnUrl)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
        );

        return LocalRedirect(returnUrl ?? "/");
    }
}
