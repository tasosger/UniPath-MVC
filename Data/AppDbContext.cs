using System;
using Microsoft.EntityFrameworkCore;
using UniPath_MVC.Models;
using UniPath_MVC.Helpers;

namespace UniPath_MVC.Data
{
    public class AppDbContext : DbContext
    {

        // regisetr classes and schema on db
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Class> Classes { get; set; }
        public DbSet<Capsule> Capsules { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }

        public DbSet<TrueFalseQuestion> TrueFalseQuestions { get; set; }
        public DbSet<CapsuleCompletion> CapsuleCompletions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CapsuleCompletion>()
                .HasKey(cc => cc.Id);

            modelBuilder.Entity<CapsuleCompletion>()
                .HasIndex(cc => new { cc.CapsuleId, cc.StudentId })
                .IsUnique();
        }

    }
}
