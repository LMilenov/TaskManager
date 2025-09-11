using TaskManager.API.DTOs;

public class CategoryReadDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int TaskCount { get; set; }
}