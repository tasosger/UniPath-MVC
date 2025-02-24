using Microsoft.AspNetCore.Http; // Required for session
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniPath_MVC.Data;
using UniPath_MVC.Models;
using UniPath_MVC.Models.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

public class CapsuleController : Controller
{
    private readonly AppDbContext _context;

    public CapsuleController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Details(int capsuleId)
    {
        var capsule = await _context.Capsules
            .Include(c => c.TrueFalseQuestions)
            .FirstOrDefaultAsync(c => c.Id == capsuleId);

        if (capsule == null)
            return NotFound();

        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return RedirectToAction("Login", "Account"); 

        var isCompleted = await _context.CapsuleCompletions
            .AnyAsync(cc => cc.CapsuleId == capsuleId && cc.StudentId == userId.Value);

        var viewModel = new CapsuleViewModel
        {
            Capsule = capsule,
            TrueFalseQuestions = capsule.TrueFalseQuestions.ToList(),
            IsCompleted = isCompleted
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitAnswer(int questionId, int capsuleId, bool answer)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return RedirectToAction("Login", "Account");

        var question = await _context.TrueFalseQuestions.FindAsync(questionId);
        if (question == null)
            return NotFound();

        bool isCorrect = question.CorrectAnswer == answer;

        TempData["IsAnswerCorrect"] = isCorrect ? "Correct!" : "Wrong. Try again.";

        return RedirectToAction("Details", "Capsule", new { capsuleId });
    }

    [HttpPost]
    [Authorize]
    public IActionResult MarkStudentComplete([FromBody] int capsuleId)
    {
        var studentId = HttpContext.Session.GetInt32("UserId");
        Console.WriteLine("here");

        if (studentId == null)
        {
            return Json(new { success = false, message = "Student not logged in." });
        }

        var existingCompletion = _context.CapsuleCompletions
            .FirstOrDefault(cc => cc.CapsuleId == capsuleId && cc.StudentId == studentId);

        if (existingCompletion == null)
        {
            var completion = new CapsuleCompletion
            {
                CapsuleId = capsuleId,
                StudentId = studentId.Value,
                IsCompleted = true
            };

            _context.CapsuleCompletions.Add(completion);
        }
        else
        {
            existingCompletion.IsCompleted = true; 
        }

        _context.SaveChanges();
        return Json(new { success = true });
    }

}
