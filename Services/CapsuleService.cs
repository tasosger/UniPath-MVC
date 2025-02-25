using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UniPath_MVC.Data;
using UniPath_MVC.Models;
using UniPath_MVC.Models.ViewModels;

namespace UniPath_MVC.Services
{
    public class CapsuleService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CapsuleService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CapsuleViewModel?> GetCapsuleDetailsAsync(int capsuleId)
        {
            var capsule = await _context.Capsules
                .Include(c => c.TrueFalseQuestions)
                .FirstOrDefaultAsync(c => c.Id == capsuleId);

            if (capsule == null) return null;

            int? studentId = _httpContextAccessor.HttpContext?.Session.GetInt32("UserId");
            if (studentId == null) return null;

            bool isCompleted = await _context.CapsuleCompletions
                .AnyAsync(cc => cc.CapsuleId == capsuleId && cc.StudentId == studentId.Value);

            return new CapsuleViewModel
            {
                Capsule = capsule,
                TrueFalseQuestions = capsule.TrueFalseQuestions.ToList(),
                IsCompleted = isCompleted
            };
        }

        public async Task<bool?> SubmitAnswerAsync(int questionId, int capsuleId, bool answer)
        {
            int? userId = _httpContextAccessor.HttpContext?.Session.GetInt32("UserId");
            if (userId == null) return null;

            var question = await _context.TrueFalseQuestions.FindAsync(questionId);
            if (question == null) return null;

            return question.CorrectAnswer == answer;
        }

        public async Task<bool> MarkStudentCompleteAsync(int capsuleId)
        {
            int? studentId = _httpContextAccessor.HttpContext?.Session.GetInt32("UserId");
            if (studentId == null) return false;

            bool studentExists = await _context.Students.AnyAsync(s => s.Id == studentId);
            bool capsuleExists = await _context.Capsules.AnyAsync(c => c.Id == capsuleId);
            if (!studentExists || !capsuleExists) return false;

            var existingCompletion = await _context.CapsuleCompletions
                .FirstOrDefaultAsync(cc => cc.CapsuleId == capsuleId && cc.StudentId == studentId);

            if (existingCompletion == null)
            {
                _context.CapsuleCompletions.Add(new CapsuleCompletion
                {
                    CapsuleId = capsuleId,
                    StudentId = studentId.Value,
                    IsCompleted = true
                });
            }
            else
            {
                existingCompletion.IsCompleted = true;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ResetCompletionAsync(int capsuleId)
        {
            int? studentId = _httpContextAccessor.HttpContext?.Session.GetInt32("UserId");
            if (studentId == null) return false;

            var existingCompletion = await _context.CapsuleCompletions
                .FirstOrDefaultAsync(cc => cc.CapsuleId == capsuleId && cc.StudentId == studentId.Value);

            if (existingCompletion != null)
            {
                _context.CapsuleCompletions.Remove(existingCompletion);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
