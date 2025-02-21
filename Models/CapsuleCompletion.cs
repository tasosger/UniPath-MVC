namespace UniPath_MVC.Models
{
    public class CapsuleCompletion
    {
        public int Id { get; set; }
        public int CapsuleId { get; set; }
        public int StudentId { get; set; }
        public bool IsCompleted { get; set; }

        public Capsule? Capsule { get; set; }
        public Student? Student { get; set; }
    }

}
