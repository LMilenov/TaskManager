using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Models;
using TaskManager.API.Data;
using TaskManager.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/categories")]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CategoryReadDto>> GetCategories()
    {
        var categories = _context.Categories
            .Include(c => c.Tasks)
            .Select(c => new CategoryReadDto
            {
                Id = c.Id,
                Name = c.Name,
                TaskCount = c.Tasks.Count
            })
            .ToList();

        return Ok(categories);
    }

    [HttpPost]
    public ActionResult<CategoryReadDto> AddCategory(CategoryCreateDto dto)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized("User ID not found in token");
        }

        var userId = int.Parse(userIdClaim.Value);

        var category = new Category
        {
            Name = dto.Name,
            UserId = userId
        };

        _context.Categories.Add(category);
        _context.SaveChanges();

        var result = new CategoryReadDto
        {
            Id = category.Id,
            Name = category.Name,
            TaskCount = 0
        };

        return Ok(result);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateCategory(int id, CategoryCreateDto dto)
    {
        var category = _context.Categories.Include(c => c.Tasks).FirstOrDefault(c => c.Id == id);
        if (category == null)
        {
            return NotFound();
        }

        category.Name = dto.Name;
        _context.SaveChanges();

        var result = new CategoryReadDto
        {
            Id = category.Id,
            Name = category.Name,
            TaskCount = category.Tasks.Count
        };

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteCategory(int id)
    {
        var category = _context.Categories.Find(id);
        if (category == null)
        {
            return NotFound();
        }

        _context.Categories.Remove(category);
        _context.SaveChanges();

        return NoContent();
    }
}
