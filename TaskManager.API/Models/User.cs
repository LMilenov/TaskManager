namespace TaskManager.API.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
    public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();

    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}