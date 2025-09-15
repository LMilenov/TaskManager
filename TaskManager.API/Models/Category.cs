namespace TaskManager.API.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = "";

    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    public int UserId { get; set; }
    public User? User { get; set; }
}
