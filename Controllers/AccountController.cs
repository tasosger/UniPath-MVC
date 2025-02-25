using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UniPath_MVC.Services;
using UniPath_MVC.Models;

namespace UniPath_MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                Console.WriteLine($"Attempting login for user: {username}");

                User? user = await _accountService.AuthenticateUserAsync(username, password);

                if (user != null)
                {
                    _accountService.SignInUser(user);
                    Console.WriteLine($"✅ Login Successful! User: {username} ({(user is Student ? "Student" : "Teacher")})");

                    return RedirectToAction("Details", "Class", new { classId = 1 });
                }

                Console.WriteLine($"Login Failed: Invalid username or password for {username}");
                ViewData["Error"] = "Invalid username or password";
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during login: {ex.Message}");
                ViewData["Error"] = "An unexpected error occurred. Please try again later.";
                return View();
            }
        }

        public IActionResult Logout()
        {
            _accountService.SignOutUser();
            return RedirectToAction("Login");
        }
    }
}
