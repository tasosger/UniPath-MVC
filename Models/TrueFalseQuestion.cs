using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniPath_MVC.Models
{
    public class TrueFalseQuestion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Question { get; set; }

        [Required]
        public bool? CorrectAnswer { get; set; } 

        public int CapsuleId { get; set; }
        [ForeignKey("CapsuleId")]
        public Capsule? Capsule { get; set; }

        public TrueFalseQuestion(string question, bool correctAnswer, int capsuleId)
        {
            Question = question;
            CorrectAnswer = correctAnswer;
            CapsuleId = capsuleId;
        }

        public TrueFalseQuestion() { }
    }
}
