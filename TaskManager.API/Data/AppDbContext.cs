using Microsoft.EntityFrameworkCore;
using TaskManager.API.Models;

namespace TaskManager.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Learning" },
            new Category { Id = 2, Name = "Projects" }
        );

        modelBuilder.Entity<TaskItem>().HasData(
            new TaskItem
            {
                Id = 1,
                Title = "Learn C#",
                Description = "Practice C# basics",
                DueDate = DateTime.Now.AddDays(3),
                IsCompleted = false,
                CategoryId = 1
            },
            new TaskItem
            {
                Id = 2,
                Title = "Build API",
                Description = "Create first Web API",
                DueDate = DateTime.Now.AddDays(5),
                IsCompleted = false,
                CategoryId = 2
            }
        );
    }
}