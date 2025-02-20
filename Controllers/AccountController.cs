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
            Console.WriteLine(password);
            string hashedPassword = PasswordHelper.ComputeSha256Hash(password);
            Console.WriteLine(hashedPassword);
            User? user = await _context.Students
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == hashedPassword);
            
            Console.WriteLine(hashedPassword);
            if (user == null)
            {
                user = await _context.Teachers
                    .FirstOrDefaultAsync(u => u.Username == username && u.Password == hashedPassword);
            }

            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("UserType", user is Student ? "Student" : "Teacher");

                //Class page to be implemented (In the complete application a user should be redirected to the home page)
                return RedirectToAction("Details", "Class", new { id = 1 });
            } else
            {
                ViewData["Error"] = "Invalid username or password";
               
                return View();
            }

            ViewData["Error"] = "Invalid username or password";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        
    }
}
