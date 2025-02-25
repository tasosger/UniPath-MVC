using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UniPath_MVC.Services;
using UniPath_MVC.Models;

namespace UniPath_MVC.Controllers
{
    public class ClassController : Controller
    {
        private readonly ClassService _classService;

        public ClassController(ClassService classService)
        {
            _classService = classService;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int classId)
        {
            var classDetails = await _classService.GetClassDetailsAsync(classId);
            if (classDetails == null)
            {
                return NotFound();
            }

            var capsules = await _classService.GetCapsulesForClassAsync(classId);
            ViewData["Capsules"] = capsules;

            return View(classDetails);
        }
    }
}
