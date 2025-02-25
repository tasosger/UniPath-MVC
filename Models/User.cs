using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using UniPath_MVC.Helpers;

namespace UniPath_MVC.Models
{

    public abstract class User
    {
        // user model
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
            Password = PasswordHelper.ComputeSha256Hash(password);
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Bio = bio;
            Console.WriteLine(Password);
        }

        public User()
        {

        }


    }
}
