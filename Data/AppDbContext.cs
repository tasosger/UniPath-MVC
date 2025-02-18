using Microsoft.EntityFrameworkCore;
using UniPath_MVC.Models;

namespace UniPath_MVC.Data

{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Capsule> Capsules { get; set; }
        public DbSet<TrueFalseQuestion> TrueFalseQuestions { get; set; }

    }
}
