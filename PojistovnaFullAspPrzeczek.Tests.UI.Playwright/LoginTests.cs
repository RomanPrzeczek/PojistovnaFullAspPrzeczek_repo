using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Playwright;

namespace PojistovnaFullAspPrzeczek.Tests.UI.Playwright
{
    [TestClass]
    public class LoginTests
    {
        private IBrowser _browser = default!;
        private IPage _page = default!;
        private IBrowserContext _context = default!;
        private IPlaywright _playwright = default!;

        [TestInitialize]
        public async Task Setup()
        {
            _playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            _context = await _browser.NewContextAsync();
            _page = await _context.NewPageAsync();
        }

        [TestMethod]
        public async Task Admin_Can_Login()
        {
            await _page.GotoAsync("https://localhost:7046/");

            await _page.FillAsync("input[name='Email']", "admin@example.com");
            await _page.FillAsync("input[name='Password']", "Admin123+");
            await _page.ClickAsync("button[type='submit']");

            // redirect of admin to InsuredPersons/Index after login, text verification/testing
            await _page.WaitForURLAsync("**/InsuredPersons");
            var heading = await _page.Locator("h2").InnerTextAsync();

            Assert.IsTrue(heading.Contains("Pojištěnci") || heading.Contains("Přehled")); // acc. setup
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            await _page.CloseAsync();
            await _context.CloseAsync();
            await _browser.CloseAsync();
            _playwright.Dispose();
        }
    }
}
