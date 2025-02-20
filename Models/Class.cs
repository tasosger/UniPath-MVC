using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniPath_MVC.Models
{
    public class Class
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public int TeacherId { get; set; }
        [ForeignKey("TeacherId")]
        public Teacher? Teacher { get; set; }

        public ICollection<Capsule>? Capsules { get; set; }
        public Class(string name, int teacherId, string? description = null)
        {
            Name = name;
            TeacherId = teacherId;
            Description = description;
            Capsules = new List<Capsule>(); 
        }
        public Class() { 
        
        }
    }
}
