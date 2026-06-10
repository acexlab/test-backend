namespace ToDoApi.DTOs;

public class SearchDto
{
    public string? SearchTerm { get; set; }

    public bool? IsCompleted { get; set; }
}