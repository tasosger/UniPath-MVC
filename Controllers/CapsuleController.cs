using Microsoft.AspNetCore.Http; // Required for session
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniPath_MVC.Data;
using UniPath_MVC.Models;
using UniPath_MVC.Models.ViewModels;
using System.Linq;
using System.Threading.Tasks;

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
    public async Task<IActionResult> MarkAsCompleted(int capsuleId)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return RedirectToAction("Login", "Account");

        var existingCompletion = await _context.CapsuleCompletions
            .FirstOrDefaultAsync(cc => cc.CapsuleId == capsuleId && cc.StudentId == userId.Value);

        if (existingCompletion == null)
        {
            var completion = new CapsuleCompletion
            {
                CapsuleId = capsuleId,
                StudentId = userId.Value,
                IsCompleted = true
            };
            _context.CapsuleCompletions.Add(completion);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Details", "Capsule", new { capsuleId });
    }
}
