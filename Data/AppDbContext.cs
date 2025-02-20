using System;
using Microsoft.EntityFrameworkCore;
using UniPath_MVC.Models;
using UniPath_MVC.Helpers;

namespace UniPath_MVC.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Class> Classes { get; set; }
        public DbSet<Capsule> Capsules { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            string teacherPassword = PasswordHelper.ComputeSha256Hash("pass123");
            string studentPassword = PasswordHelper.ComputeSha256Hash("pass123");

            var teacher = new Teacher { Id = 1, Username = "teacher1", Password = "pass123", FirstName = "John", LastName = "Doe", Email = "john.doe@unipath.com" };
            var student = new Student { Id = 2, Username = "student1", Password = "pass123", FirstName = "Alice", LastName = "Smith", Email = "alice.smith@unipath.com" };

            var mathClass = new Class { Id = 1, Name = "Math 101", Description = "Basic Math for Beginners", TeacherId = 1 };
            var capsule = new Capsule { Id = 1, Title = "Introduction to Algebra", Description = "Learn basic algebraic concepts", ClassId = 1 };

            modelBuilder.Entity<Teacher>().HasData(teacher);
            modelBuilder.Entity<Student>().HasData(student);
            modelBuilder.Entity<Class>().HasData(mathClass);
            modelBuilder.Entity<Capsule>().HasData(capsule);

            
        }
    }
}
