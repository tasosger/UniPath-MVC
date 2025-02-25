using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UniPath_MVC.Data;
using UniPath_MVC.Models;
using UniPath_MVC.Helpers;

namespace UniPath_MVC.Services
{
    public class AccountService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // auth method
        public async Task<User?> AuthenticateUserAsync(string username, string password)
        {
            // hash password
            string hashedPassword = PasswordHelper.ComputeSha256Hash(password);

            User? user = await _context.Students
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == hashedPassword);

            if (user == null)
            {
                user = await _context.Teachers
                    .FirstOrDefaultAsync(u => u.Username == username && u.Password == hashedPassword);
            }

            return user;
        }

        public void SignInUser(User user)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                session.SetString("Username", user.Username);
                session.SetString("UserType", user is Student ? "Student" : "Teacher");
                session.SetInt32("UserId", user.Id);
            }
        }

        public void SignOutUser()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            session?.Clear();
        }
    }
}
