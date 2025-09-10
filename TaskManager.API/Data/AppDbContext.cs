using Microsoft.EntityFrameworkCore;
using TaskManager.API.Models;

namespace TaskManager.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TaskItem> Tasks => Set<TaskItem>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TaskItem>().HasData(
            new TaskItem
            {
                Id = 1,
                Title = "Learn C#",
                Description = "Practice C# basics",
                DueDate = DateTime.Now.AddDays(3),
                IsCompleted = false
            },
            new TaskItem
            {
                Id = 2,
                Title = "Build API",
                Description = "Create first Web API",
                DueDate = DateTime.Now.AddDays(5),
                IsCompleted = false
            }
        );
    }
}