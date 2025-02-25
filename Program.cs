using Microsoft.EntityFrameworkCore;
using System;
using UniPath_MVC.Data;
using UniPath_MVC.Helpers;
using UniPath_MVC.Models;




var builder = WebApplication.CreateBuilder(args);



//In memory DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();
app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
    {
        context.Response.Redirect("/Home/PageNotFound");
    }
});


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();

        Console.WriteLine("Deleting & Recreating Database...");

        context.Database.EnsureDeleted(); 

        context.Database.EnsureCreated();

        Console.WriteLine("Database Recreated Successfully!");

        SeedDatabase(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error recreating the database: {ex.Message}");
    }
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Use(async (context, next) =>
{
    await next(); 

    if (context.Response.StatusCode == 404)
    {
        context.Request.Path = "/Home/NotFound";
        await next();
    }
});



app.Run();


static void SeedDatabase(AppDbContext context)
{
    
    Console.WriteLine("Seeding Database...");

    string teacherPassword = PasswordHelper.ComputeSha256Hash("pass123");
    string studentPassword = PasswordHelper.ComputeSha256Hash("pass123");

    var teacher = new Teacher { Id = 1, Username = "teacher1", Password = teacherPassword, FirstName = "John", LastName = "Doe", Email = "john.doe@unipath.com" };
    var student = new Student { Id = 2, Username = "student1", Password = studentPassword, FirstName = "Alice", LastName = "Smith", Email = "alice.smith@unipath.com" };

    context.Teachers.Add(teacher);
    context.Students.Add(student);
    var mathClass = new Class { Id = 1, Name = "Math 101", Description = "Basic Math for Beginners", TeacherId = 1 };
    var capsule = new Capsule { Id = 1, Title = "Introduction to Algebra", Description = "Learn basic algebraic concepts", ClassId = 1 };
    context.Classes.Add(mathClass);
    context.Capsules.Add(capsule);


    var questions = new List<TrueFalseQuestion>
        {
            new TrueFalseQuestion { Id = 1, CapsuleId = 1, Question = "Algebra is a branch of mathematics that deals with symbols.", CorrectAnswer = true },
            new TrueFalseQuestion { Id = 2, CapsuleId = 1, Question = "In algebra, 'x' always represents a number equal to 10.", CorrectAnswer = false },
            new TrueFalseQuestion { Id = 3, CapsuleId = 1, Question = "An equation is a mathematical statement that asserts the equality of two expressions.", CorrectAnswer = true }
        };

    context.TrueFalseQuestions.AddRange(questions);
    context.SaveChanges();

    Console.WriteLine("Database Seeding Completed!");
    
}