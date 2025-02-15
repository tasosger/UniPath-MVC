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
        public string? Password;

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        public string? Bio { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

          

    }
}
