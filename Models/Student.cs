using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniPath_MVC.Models
{
    
    public class Student: User
    {
        public string? LevelOfEducation { get; set; }

        public Student(string username, string password, string firstName, string lastName, string email,
                       string? levelOfEducation = null, string? bio = null)
            : base(username, password, firstName, lastName, email, bio)
        {
            LevelOfEducation = levelOfEducation;
        }

        public Student()
        {

        }
    }
}
