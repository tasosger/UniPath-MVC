using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UniPath_MVC.Services;
using UniPath_MVC.Models;
using UniPath_MVC.Models.ViewModels;

public class CapsuleController : Controller
{
    private readonly CapsuleService _capsuleService;

    public CapsuleController(CapsuleService capsuleService)
    {
        _capsuleService = capsuleService;
    }

    public async Task<IActionResult> Details(int capsuleId)
    {
        var viewModel = await _capsuleService.GetCapsuleDetailsAsync(capsuleId);
        if (viewModel == null) return RedirectToAction("Login", "Account");

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitAnswer(int questionId, int capsuleId, bool answer)
    {
        bool? isCorrect = await _capsuleService.SubmitAnswerAsync(questionId, capsuleId, answer);
        if (isCorrect == null) return RedirectToAction("Login", "Account");

        TempData["IsAnswerCorrect"] = isCorrect.Value ? "Correct!" : "Wrong. Try again.";
        return RedirectToAction("Details", new { capsuleId });
    }

    [HttpPost]
    public async Task<IActionResult> MarkStudentComplete([FromBody] CapsuleRequest request)
    {
        bool success = await _capsuleService.MarkStudentCompleteAsync(request.capsuleId);
        return Json(new { success, message = success ? "Marked as completed" : "Failed to mark complete", error = success ? null : "INTERNAL_ERROR" });
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> ResetCompletion([FromBody] CapsuleRequest request)
    {
        bool success = await _capsuleService.ResetCompletionAsync(request.capsuleId);
        return Json(new { success, message = success ? "Completion reset" : "Failed to reset", error = success ? null : "INTERNAL_ERROR" });
    }
}

public class CapsuleRequest
{
    public int capsuleId { get; set; }
}



