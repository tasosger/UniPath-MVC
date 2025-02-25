using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UniPath_MVC.Data;
using UniPath_MVC.Models;

namespace UniPath_MVC.Services
{
    public class TrueFalseQuestionService
    {
        private readonly AppDbContext _context;

        public TrueFalseQuestionService(AppDbContext context)
        {
            _context = context;
        }

        // get a specific question
        public async Task<TrueFalseQuestion?> GetQuestionByIdAsync(int questionId)
        {
            return await _context.TrueFalseQuestions.FirstOrDefaultAsync(q => q.Id == questionId);
        }

        // check answer
        public bool ValidateAnswer(TrueFalseQuestion question, bool answer)
        {
            return question.CorrectAnswer == answer;
        }
    }
}
