using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManager.API.Controllers;
using TaskManager.API.Data;
using TaskManager.API.DTOs;
using TaskManager.API.Models;
using Xunit;

namespace TaskManager.Tests;

public class TaskControllerTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    private TaskController GetControllerWithUser(AppDbContext context)
    {
        var controller = new TaskController(context);

        context.Users.Add(new User { Id = 1, Username = "TestUser" });

        context.Categories.Add(new Category { Id = 1, Name = "Default", UserId = 1 });
        context.SaveChanges();

        var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "1") };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };

        return controller;
    }

    [Fact]
    public void UpdateTask_ShouldModifyTask_WhenExists()
    {
        var context = GetInMemoryDbContext();
        var controller = GetControllerWithUser(context);

        var dto = new TaskCreateDto
        {
            Title = "Original Title",
            Description = "Original Description",
            DueDate = DateTime.UtcNow.AddDays(1),
            IsCompleted = false,
            CategoryId = 1
        };
        controller.AddTask(dto);
        var task = context.Tasks.First();

        var updated = new TaskCreateDto
        {
            Title = "Updated Title",
            Description = "Updated Description",
            DueDate = DateTime.UtcNow.AddDays(2),
            IsCompleted = true,
            CategoryId = 1
        };
        controller.UpdateTask(task.Id, updated);

        var modified = context.Tasks.Find(task.Id);
        Assert.Equal("Updated Title", modified!.Title);
        Assert.Equal("Updated Description", modified.Description);
        Assert.True(modified.IsCompleted);
    }

    [Fact]
    public void DeleteTask_ShouldRemoveTask_WhenExists()
    {
        var context = GetInMemoryDbContext();
        var controller = GetControllerWithUser(context);

        var dto = new TaskCreateDto
        {
            Title = "Task to delete",
            Description = "Will be removed",
            DueDate = DateTime.UtcNow.AddDays(1),
            IsCompleted = false,
            CategoryId = 1
        };
        controller.AddTask(dto);
        var task = context.Tasks.First();

        controller.DeleteTask(task.Id);

        var deleted = context.Tasks.Find(task.Id);
        Assert.Null(deleted);
        Assert.Empty(context.Tasks.ToList());
    }
}
