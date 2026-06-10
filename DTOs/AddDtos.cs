namespace ToDoApi.DTOs;

public class AddDtos
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Priority { get; set; } = "Medium";
}