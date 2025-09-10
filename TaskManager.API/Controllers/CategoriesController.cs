using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Models;
using TaskManager.API.Data;

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
    public ActionResult<Category> AddCategory(Category category)
    {
        _context.Categories.Add(category);
        _context.SaveChanges();
        return Ok(category);
    }
}