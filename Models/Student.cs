using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniPath_MVC.Models
{
    
    public class Student: User
    {
        public string? LevelOfEducation { get; set; }

    }
}
