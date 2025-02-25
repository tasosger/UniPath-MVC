using Microsoft.EntityFrameworkCore;
using UniPath_MVC.Data;
namespace UniPath_MVC.Models

{
    public class CapsuleCompletion
    {
        // capsule completion model
        public int Id { get; set; }
        public int CapsuleId { get; set; }
        public int StudentId { get; set; }
        public bool IsCompleted { get; set; }

        public Capsule? Capsule { get; set; }
        public Student? Student { get; set; }

        private readonly AppDbContext _context; 


        

    }

}
