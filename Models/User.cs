using System;
using System.ComponentModel.DataAnnotations;

namespace UniPath_MVC.Models
{

    public abstract class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Username { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        public string? Bio { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        public User(string username, string password, string firstName, string lastName, string email, string? bio = null)
        {
            Username = username;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Bio = bio;
        }

        public User()
        {

        }


    }
}
