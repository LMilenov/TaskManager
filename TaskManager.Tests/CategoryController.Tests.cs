using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.API.Controllers;
using TaskManager.API.Data;
using TaskManager.API.DTOs;
using TaskManager.API.Models;
using Xunit;

namespace TaskManager.Tests;

public class CategoriesControllerTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        return new AppDbContext(options);
    }

    [Fact]
    public void AddCategory_ShouldSaveCategoryInDatabase()
    {
        var context = GetInMemoryDbContext();
        var controller = new CategoriesController(context);

        var dto = new CategoryCreateDto { Name = "Work" };

        controller.AddCategory(dto);
        var categories = context.Categories.ToList();

        Assert.Single(categories);
        Assert.Equal("Work", categories[0].Name);
    }

    [Fact]
    public void DeleteCategory_ShouldRemoveCategory_WhenExists()
    {
        var context = GetInMemoryDbContext();
        var controller = new CategoriesController(context);

        var category = new Category { Name = "Temp" };
        context.Categories.Add(category);
        context.SaveChanges();

        controller.DeleteCategory(category.Id);

        Assert.Empty(context.Categories.ToList());
    }

    [Fact]
    public void DeleteCategory_ReturnsNotFound_WhenCategoryDoesNotExist()
    {
        var context = GetInMemoryDbContext();
        var controller = new CategoriesController(context);

        var result = controller.DeleteCategory(999);

        Assert.IsType<NotFoundResult>(result);
    }
}