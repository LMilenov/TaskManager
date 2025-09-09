using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Models;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private static List<TaskItem> tasks = new()
    {
        new TaskItem { Id = 1, Title = "Learn C#", Description = "Practise C# basics", DueDate = DateTime.Now.AddDays(3), IsCompleted = false},
        new TaskItem { Id = 2, Title = "Build API", Description = "Create first Web API", DueDate = DateTime.Now.AddDays(5), IsCompleted = false}
    };

    // GET api/tasks
    [HttpGet]
    public ActionResult<IEnumerable<TaskItem>> GetTasks()
    {
        return Ok(tasks);
    }

    // Post api/tasks
    [HttpPost]
    public ActionResult<TaskItem> AddTask(TaskItem task)
    {
        task.Id = tasks.Count + 1;
        tasks.Add(task);
        return Ok(task);
    }
}