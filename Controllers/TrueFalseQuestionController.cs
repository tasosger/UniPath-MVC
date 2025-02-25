using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UniPath_MVC.Services;

namespace UniPath_MVC.Controllers
{
    public class TrueFalseQuestionController : Controller
    {
        private readonly TrueFalseQuestionService _questionService;

        public TrueFalseQuestionController(TrueFalseQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitAnswer(int questionId, int capsuleId, bool answer)
        {
            var question = await _questionService.GetQuestionByIdAsync(questionId);
            if (question == null)
            {
                return NotFound();
            }

            bool isCorrect = _questionService.ValidateAnswer(question, answer);
            TempData["isAnswerCorrect"] = isCorrect ? "Correct!" : "Wrong. Try again.";

            return RedirectToAction("Details", "Capsule", new { capsuleId });
        }
    }
}
