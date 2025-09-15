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

public class CategoriesControllerTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    private CategoriesController GetControllerWithUser(AppDbContext context)
    {
        var controller = new CategoriesController(context);

        context.Users.Add(new User { Id = 1, Username = "TestUser" });
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
    public void AddCategory_ShouldSaveCategoryInDatabase()
    {
        var context = GetInMemoryDbContext();
        var controller = GetControllerWithUser(context);

        var dto = new CategoryCreateDto { Name = "Work" };

        var result = controller.AddCategory(dto);

        var categories = context.Categories.ToList();
        Assert.Single(categories);
        Assert.Equal("Work", categories[0].Name);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var categoryDto = Assert.IsType<CategoryReadDto>(okResult.Value);
        Assert.Equal("Work", categoryDto.Name);
    }

    [Fact]
    public void AddCategory_ReturnsBadRequest_WhenNameIsEmpty()
    {
        var context = GetInMemoryDbContext();
        var controller = GetControllerWithUser(context);

        var dto = new CategoryCreateDto { Name = "" };

        var result = controller.AddCategory(dto);

        controller.ModelState.AddModelError("Name", "Required");

        Assert.False(controller.ModelState.IsValid);
    }

    [Fact]
    public void UpdateCategory_ReturnsNotFound_WhenCategoryDoesNotExist()
    {
        var context = GetInMemoryDbContext();
        var controller = GetControllerWithUser(context);

        var dto = new CategoryCreateDto { Name = "Updated" };

        var result = controller.UpdateCategory(99, dto);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void DeleteCategory_ShouldRemoveCategory_WhenExists()
    {
        var context = GetInMemoryDbContext();
        var controller = GetControllerWithUser(context);

        var category = new Category { Id = 1, Name = "Temp", UserId = 1 };
        context.Categories.Add(category);
        context.SaveChanges();

        var result = controller.DeleteCategory(1);

        Assert.IsType<NoContentResult>(result);
        Assert.Empty(context.Categories.ToList());
    }

    [Fact]
    public void DeleteCategory_ReturnsNotFound_WhenCategoryDoesNotExist()
    {
        var context = GetInMemoryDbContext();
        var controller = GetControllerWithUser(context);

        var result = controller.DeleteCategory(999);

        Assert.IsType<NotFoundResult>(result);
    }
}
