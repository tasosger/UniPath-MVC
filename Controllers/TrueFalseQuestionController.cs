using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniPath_MVC.Data;
using UniPath_MVC.Models;
using System.Linq;

namespace UniPath_MVC.Controllers
{
    public class TrueFalseQuestionController : Controller
    {
        private readonly AppDbContext _context;

        public TrueFalseQuestionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult SubmitAnswer(int questionId, int capsuleId, bool answer)
        {
            var question = _context.TrueFalseQuestions.FirstOrDefault(q => q.Id == questionId);
            if (question == null)
            {
                return NotFound();
            }

            bool isCorrect = question.CorrectAnswer == answer;

            TempData["isAnswerCorrect"] = isCorrect ? "Correct!" : "Wrong. Try again.";

            return RedirectToAction("Details", "Capsule", new { capsuleId });
        }
    }
}
