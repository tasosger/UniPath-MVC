using System.Collections.Generic;
using UniPath_MVC.Models;

namespace UniPath_MVC.Models.ViewModels
{
    public class CapsuleViewModel
    {
        // model for the capsule details page
        public Capsule? Capsule { get; set; } 
        public List<TrueFalseQuestion>? TrueFalseQuestions { get; set; }
        public bool IsCompleted { get; set; }

        public List<Capsule>? CapsulesInClass { get; set; }

        public ICollection<CapsuleCompletion>? CapsuleCompletions { get; set; }
    }
}
