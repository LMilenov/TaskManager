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
    public ActionResult<IEnumerable<Category>> GetCategories()
    {
        return Ok(_context.Categories.ToList());
    }

    [HttpPost]
    public ActionResult<Category> AddCategory(CategoryCreateDto dto)
    {
        var category = new Category { Name = dto.Name };
        _context.Categories.Add(category);
        _context.SaveChanges();
        return Ok(category);
    }
}