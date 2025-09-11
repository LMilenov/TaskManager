using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Models;
using TaskManager.API.Data;
using TaskManager.API.DTOs;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/categories")]
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
        var categories = _context.Categories.Select(c => new CategoryReadDto
        {
            Id = c.Id,
            Name = c.Name,
            TaskCount = c.Tasks.Count,
        })
        .ToList();

        return Ok(categories);
    }

    [HttpPost]
    public ActionResult<Category> AddCategory(CategoryCreateDto dto)
    {
        var category = new Category { Name = dto.Name };
        _context.Categories.Add(category);
        _context.SaveChanges();
        return Ok(new CategoryReadDto
        {
            Id = category.Id,
            Name = category.Name,
            TaskCount = 0
        });
    }
}