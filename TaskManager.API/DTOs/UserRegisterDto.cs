using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.DTOs;

public class UserRegisterDto
{
    [Required]
    [MinLength(3)]
    public string Username { get; set; } = "";

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = "";
}