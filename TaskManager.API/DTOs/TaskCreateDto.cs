using System.ComponentModel.DataAnnotations;
using TaskManager.API.Validation;

namespace TaskManager.API.DTOs;

public class TaskCreateDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, ErrorMessage = "Tittle cannot be longer that 100 characters")]
    public string Title { get; set; } = "";

    [Required(ErrorMessage = "Description is required")]
    [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
    public string Description { get; set; } = "";

    [Required]
    [DataType(DataType.Date)]
    [FutureDate(ErrorMessage = "Due date must be in the future")]
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }

    [Required(ErrorMessage = "CategoryId is required")]
    public int CategoryId { get; set; }
}