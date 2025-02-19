using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniPath_MVC.Data;
using UniPath_MVC.Models;

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
            //Passwords are hashed for security perposes
            string hashedPassword = ComputeSha256Hash(password);

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

                //Class page to be implemented (In the complete application a user should be redirected to the home page)
                return RedirectToAction("Index", "Class");

            }

            ViewData["Error"] = "Invalid username or password";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        private static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                foreach (byte t in bytes)
                {
                    builder.Append(t.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
