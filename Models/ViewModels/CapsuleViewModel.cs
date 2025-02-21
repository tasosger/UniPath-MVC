using System.Collections.Generic;
using UniPath_MVC.Models;

namespace UniPath_MVC.Models.ViewModels
{
    public class CapsuleViewModel
    {
        public Capsule? Capsule { get; set; } 
        public List<TrueFalseQuestion>? TrueFalseQuestions { get; set; }
        public bool IsCompleted { get; set; }

        public List<Capsule>? CapsulesInClass { get; set; }
    }
}
