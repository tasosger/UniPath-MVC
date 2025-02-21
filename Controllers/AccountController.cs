using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniPath_MVC.Data;
using UniPath_MVC.Models;
using UniPath_MVC.Helpers;


namespace UniPath_MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
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
                Console.WriteLine($"Entered Password: {password}");

                string hashedPassword = PasswordHelper.ComputeSha256Hash(password);
                Console.WriteLine($"Hashed Password: {hashedPassword}");

                User? user = await _context.Students
                    .FirstOrDefaultAsync(u => u.Username == username && u.Password == hashedPassword);

                if (user == null)
                {
                    user = await _context.Teachers
                        .FirstOrDefaultAsync(u => u.Username == username && u.Password == hashedPassword);
                }

                if (user != null)
                {
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("UserType", user is Student ? "Student" : "Teacher");

                    Console.WriteLine($"✅ Login Successful! User: {username} ({(user is Student ? "Student" : "Teacher")})");

                    int classId = 1;  
                    Console.WriteLine($"Redirecting {username} to /Class/Details?classId={classId}");
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    return RedirectToAction("Details", "Class", new { classId });
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
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        
    }
}
