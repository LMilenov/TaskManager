using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.DTOs;

public class CategoryCreateDto
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters")]
    public string Name { get; set; } = "";
}