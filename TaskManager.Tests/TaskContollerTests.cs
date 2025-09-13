using Microsoft.EntityFrameworkCore;
using TaskManager.API.Controllers;
using TaskManager.API.Data;
using TaskManager.API.DTOs;
using TaskManager.API.Models;
using Xunit;

namespace TaskManager.Tests;

public class TaskContollerTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        return new AppDbContext(options);
    }

    [Fact]
    public void UpdateTask_ShouldModifyTask_WhenExists()
    {
        var context = GetInMemoryDbContext();
        var controller = new TaskController(context);

        var dto = new TaskCreateDto
        {
            Title = "Original Title",
            Description = "Original Description",
            DueDate = DateTime.Now.AddDays(1),
            IsCompleted = false,
            CategoryId = 1
        };

        controller.AddTask(dto);
        var task = context.Tasks.First();

        var updated = new TaskItem
        {
            Id = task.Id,
            Title = "Updated Title",
            Description = "Updated Description",
            DueDate = DateTime.Now.AddDays(2),
            IsCompleted = true,
            CategoryId = 1
        };

        controller.UpdateTask(task.Id, updated);

        var modified = context.Tasks.Find(task.Id);
        Assert.Equal("Updated Title", modified!.Title);
        Assert.Equal("Updated Description", modified!.Description);
        Assert.True(modified.IsCompleted);
    }

    [Fact]
    public void DeleteTask_ShouldRemoveTask_WhenExists()
    {
        var context = GetInMemoryDbContext();
        var controller = new TaskController(context);

        var dto = new TaskCreateDto
        {
            Title = "Task to delete",
            Description = "Will be removed",
            DueDate = DateTime.Now.AddDays(1),
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