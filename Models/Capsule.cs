using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace UniPath_MVC.Models
{
    public class Capsule
    {
        [Key]
        public int Id { get; set; }


        [Required]
        public string? Title { get; set; }

        public string? VideoURL { get; set; }

        public string? Description { get; set; }

        public string? Content { get; set; }

        public int ClassId { get; set; }
        [ForeignKey("ClassId")]
        public Class? Class { get; set; }
        public Capsule(string title, int classId, string? description = null, string? content = null, string? videoURL = null)
        {
            Title = title;
            ClassId = classId;
            Description = description;
            Content = content;
            VideoURL = videoURL;
        }

        public Capsule() { 
        
        }
    }
}
