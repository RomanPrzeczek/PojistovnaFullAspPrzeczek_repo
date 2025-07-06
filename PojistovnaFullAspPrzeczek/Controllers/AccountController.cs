using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using PojistovnaFullAspPrzeczek.Data;
using PojistovnaFullAspPrzeczek.Models;
using PojistovnaFullAspPrzeczek.ViewModels;
using System.Diagnostics;
using Microsoft.Extensions.Localization;

namespace PojistovnaFullAspPrzeczek.Controllers
{
    public class AccountController (
            UserManager<IdentityUser> _userManager,
            SignInManager<IdentityUser> _signInManager,
            ApplicationDbContext _context,
            IStringLocalizerFactory _localizerFactory
        ) : Controller
    {

        private readonly IStringLocalizer _sharedLocalizer =
        _localizerFactory.Create("SharedResources", typeof(Program).Assembly.GetName().Name);
        // ===== LOGIN z homepage =====

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            Debug.WriteLine(">>> Login controller reached.");

            if (!ModelState.IsValid)
            {
                Debug.WriteLine("ModelState not valid.");
                return View("~/Views/Home/Index.cshtml", model); // login je na home stránce
            }

            Debug.WriteLine($"Pokouším se přihlásit jako: {model.Email}");

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);

            Debug.WriteLine($"Výsledek přihlášení: {result.Succeeded}");

            if (result.Succeeded)
            {
                Debug.WriteLine($"Login e-mail: {model.Email}");
                return RedirectToAction("PostLoginRedirect", "Account");
            }

            Debug.WriteLine("Pojištěnec nenalezen.");
            ModelState.AddModelError(string.Empty, _sharedLocalizer["LoginFailed"].Value);

            return View("~/Views/Home/Index.cshtml", model);
        }

        // ===== PŘESMĚROVÁNÍ PO PŘIHLÁŠENÍ =====
        /// <summary>
        /// Redirects user after login based on their role.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> PostLoginRedirect()
        {
            if (!User.Identity.IsAuthenticated)
            {
                Debug.WriteLine("User is NOT authenticated.");
                return RedirectToAction("Index", "Home");
            }
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                Debug.WriteLine("User is null from UserManager.");
                    return RedirectToAction("Index", "Home");            }

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return RedirectToAction("Index", "InsuredPersons");
            }
            else if (await _userManager.IsInRoleAsync(user, "InsuredUser"))
            {
                // Předpoklad: uživatel propojen s insuredPerson podle emailu
                var insuredPerson = _context.InsuredPerson
                    .FirstOrDefault(p => p.Email == user.Email);    

                if (insuredPerson != null)
                {
                    return RedirectToAction("Details", "InsuredPersons", new { id = insuredPerson.IdInsuredPerson });
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else // general nebo žádná role
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // ===== REGISTRACE =====

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Přidání role pojištěnce
                await _userManager.AddToRoleAsync(user, "insuredPerson");

                // Zápis do tabulky pojištěnců
                var insured = new InsuredPerson
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    City = model.City,
                    Street = model.Street,
                    ZIP = model.ZIP,
                    Phone = model.Phone,
                    Email = model.Email
                };

                _context.InsuredPerson.Add(insured);
                await _context.SaveChangesAsync();

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("PostLoginRedirect", "Account");
                //return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        // ===== ODHLÁŠENÍ =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // ===== ZMĚNA JAZYKA =====
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
}
