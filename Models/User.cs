namespace TodoApi.Models;

public class User
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    // Navigation Property
    public ICollection<TodoItem> Todos { get; set; } = new List<TodoItem>();
}