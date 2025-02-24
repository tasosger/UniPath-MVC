using Microsoft.AspNetCore.Http;
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

        string sqlQuery = @"
            SELECT COUNT(*) FROM CapsuleCompletions 
            WHERE CapsuleId = {0} AND StudentId = {1}";
        int? studentId = HttpContext.Session.GetInt32("UserId");
        Console.WriteLine($"{capsuleId}- {studentId}");
        int completionCount = await _context.CapsuleCompletions
            .FromSqlRaw(sqlQuery, capsuleId, studentId.Value).CountAsync();

        bool isCompleted = completionCount > 0;

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
    public async Task<IActionResult> MarkStudentComplete([FromBody] CapsuleRequest request)
    {
        try
        {
            Console.WriteLine($"Received request for capsuleId: {request.capsuleId}");

            var studentId = HttpContext.Session.GetInt32("UserId");
            Console.WriteLine($"Current studentId from session: {studentId}");

            if (studentId == null)
            {
                Console.WriteLine("No student ID found in session");
                return Json(new { success = false, message = "Student not logged in.", error = "NO_SESSION" });
            }

            var studentExists = _context.Students.Any(s => s.Id == studentId);
            Console.WriteLine($"Student exists: {studentExists}");
            if (!studentExists)
            {
                return Json(new { success = false, message = "Student not found.", error = "NO_STUDENT" });
            }

            var capsuleExists = _context.Capsules.Any(c => c.Id == request.capsuleId);
            Console.WriteLine($"Capsule exists: {capsuleExists}");
            if (!capsuleExists)
            {
                return Json(new { success = false, message = "Capsule not found.", error = "NO_CAPSULE" });
            }

            var existingCompletion = _context.CapsuleCompletions
                .FirstOrDefault(cc => cc.CapsuleId == request.capsuleId && cc.StudentId == studentId);
            Console.WriteLine($"Existing completion found: {existingCompletion != null}");

            if (existingCompletion == null)
            {
                CapsuleCompletion completion = new CapsuleCompletion
                {
                    CapsuleId = request.capsuleId,
                    StudentId = studentId.Value,
                    IsCompleted = true
                };
                _context.CapsuleCompletions.Add(completion);
                Console.WriteLine($"Added new completion record capsuleId: - {completion.CapsuleId}, {completion.StudentId}");
            }
            else
            {
                existingCompletion.IsCompleted = true;
                Console.WriteLine("Updated existing completion record");
            }

            _context.SaveChanges();
            string sqlQuery = @"
            SELECT COUNT(*) FROM CapsuleCompletions 
            WHERE CapsuleId = {0} AND StudentId = {1}";
            Console.WriteLine($"{request.capsuleId}- {studentId}");
            int completionCount = await _context.CapsuleCompletions
            .FromSqlRaw(sqlQuery, request.capsuleId, studentId.Value)
            .CountAsync();

            bool isCompleted = completionCount > 0;
            Console.WriteLine(isCompleted);
            Console.WriteLine("Changes saved successfully");
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in MarkStudentComplete: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            return Json(new
            {
                success = false,
                message = ex.Message,
                error = "INTERNAL_ERROR"
            });
        }
    }



        
    }


    public class CapsuleRequest
    {
        public int capsuleId { get; set; }
    }


