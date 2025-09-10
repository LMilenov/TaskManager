namespace TaskManager.API.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = "";

    // навигационно свойство – една категория има много задачи
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
