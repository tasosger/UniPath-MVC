namespace UniPath_MVC.Models
{
    public class Teacher: User
    {
        // teacher model, inherits from use
        public string? Specialization { get; set; }
        public string? University { get; set; }
        public string? Hierarchy { get; set; }

        public Teacher(string username, string password, string firstName, string lastName, string email,
                       string? specialization = null, string? university = null, string? hierarchy = null, string? bio = null)
            : base(username, password, firstName, lastName, email, bio)
        {
            Specialization = specialization;
            University = university;
            Hierarchy = hierarchy;
        }

        public Teacher()
        {

        }
    }
}
