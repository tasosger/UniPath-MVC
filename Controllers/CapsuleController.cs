using Microsoft.AspNetCore.Mvc;
using UniPath_MVC.Data;

namespace UniPath_MVC.Controllers
{
    [Route("Capsule")]
    public class CapsuleController : Controller
    {
        private readonly AppDbContext _context;

        public CapsuleController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ListByClass(int classId)
        {
            var capsules = _context.Capsules
                .Where(c => c.ClassId == classId)
                .ToList();

            return PartialView("_CapsuleList", capsules);
        }

        public IActionResult Details(int capsuleId)
        {
            var capsule = _context.Capsules
                .FirstOrDefault(c => c.Id == capsuleId);

            if (capsule == null)
            {
                return NotFound();
            }

            return View(capsule);
        }

    }

}
