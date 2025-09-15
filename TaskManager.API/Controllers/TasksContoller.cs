using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.API.Data;
using TaskManager.API.DTOs;
using TaskManager.API.Models;
using System.Security.Claims;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/tasks")]
[Authorize]
public class TaskController : ControllerBase
{
    private readonly AppDbContext _context;

    public TaskController(AppDbContext context)
    {
        _context = context;
    }

    private int? GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null) return null;
        if (int.TryParse(claim.Value, out var id)) return id;
        return null;
    }

    // GET api/tasks
    [HttpGet]
    public ActionResult<IEnumerable<TaskReadDto>> GetTasks()
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized("User ID not found in token");

        var tasks = _context.Tasks
            .Where(t => t.UserId == userId) // 👈 филтър по user
            .Include(t => t.Category)
            .Select(t => new TaskReadDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                IsCompleted = t.IsCompleted,
                CategoryName = t.Category != null ? t.Category.Name : ""
            })
            .ToList();

        return Ok(tasks);
    }

    // POST api/tasks
    [HttpPost]
    public ActionResult<TaskItem> AddTask(TaskCreateDto dto)
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized("User ID not found in token");

        var category = _context.Categories.FirstOrDefault(c => c.Id == dto.CategoryId && c.UserId == userId);
        if (category == null) return BadRequest("Invalid category for current user");

        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            IsCompleted = dto.IsCompleted,
            CategoryId = dto.CategoryId,
            UserId = userId.Value
        };

        _context.Tasks.Add(task);
        _context.SaveChanges();

        var result = new TaskReadDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            IsCompleted = task.IsCompleted,
            CategoryName = task.Category?.Name
        };

        return Ok(result);
    }

    // PUT api/tasks/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateTask(int id, TaskCreateDto dto)
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized("User ID not found in token");

        var task = _context.Tasks.FirstOrDefault(t => t.Id == id && t.UserId == userId);
        if (task == null)
        {
            return NotFound();
        }

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.DueDate = dto.DueDate;
        task.IsCompleted = dto.IsCompleted;

        _context.SaveChanges();

        var result = new TaskReadDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            IsCompleted = task.IsCompleted,
            CategoryName = task.Category?.Name ?? ""
        };

        return Ok(result);
    }

    // DELETE api/tasks/{id}
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
