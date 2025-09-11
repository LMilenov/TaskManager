using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.API.Data;
using TaskManager.API.DTOs;
using TaskManager.API.Models;

namespace TaskManager.API.Controllers;




[ApiController]
[Route("api/tasks")]
public class TaskController : ControllerBase
{
    private readonly AppDbContext _context;

    public TaskController(AppDbContext context)
    {
        _context = context;
    }

    // GET api/tasks
    [HttpGet]
    public ActionResult<IEnumerable<TaskReadDto>> GetTasks()
    {
        var tasks = _context.Tasks.Include(t => t.Category).Select(t => new TaskReadDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            DueDate = t.DueDate,
            IsCompleted = t.IsCompleted,
            CategoryName = t.Category != null ? t.Category.Name : "",
        })
        .ToList();

        return Ok(tasks);
    }

    // Post api/tasks
    [HttpPost]
    public ActionResult<TaskItem> AddTask(TaskCreateDto dto)
    {
        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            IsCompleted = dto.IsCompleted,
            CategoryId = dto.CategoryId
        };
        _context.Tasks.Add(task);
        _context.SaveChanges();
        return Ok(task);
    }

    // PUT api/tasks/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateTask(int id, TaskItem updatedTask)
    {
        var task = _context.Tasks.Find(id);
        if (task == null)
        {
            return NotFound();
        }

        task.Title = updatedTask.Title;
        task.Description = updatedTask.Description;
        task.DueDate = updatedTask.DueDate;
        task.IsCompleted = updatedTask.IsCompleted;

        _context.SaveChanges();
        return Ok(task);
    }

    // Delete api/tasks/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteTask(int id)
    {
        var task = _context.Tasks.Find(id);
        if (task == null)
        {
            return NotFound();
        }

        _context.Tasks.Remove(task);
        _context.SaveChanges();
        return NoContent();
    }
}