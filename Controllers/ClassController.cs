using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniPath_MVC.Data;
using UniPath_MVC.Models;
using System.Threading.Tasks;

namespace UniPath_MVC.Controllers
{
    public class ClassController : Controller
    {
        private readonly AppDbContext _context;

        public ClassController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Details(int classId)
        {
            var classDetails = _context.Classes
                .Include(c => c.Teacher) 
                .FirstOrDefault(c => c.Id == classId);

            if (classDetails == null)
            {
                return NotFound();
            }

            var capsules = _context.Capsules
                .Where(c => c.ClassId == classId)
                .ToList();

            ViewData["Capsules"] = capsules;

            return View(classDetails);
        }

    }


}

