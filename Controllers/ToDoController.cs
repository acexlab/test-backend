using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using ToDoApi.DTOs;
using ToDoApi.Models;

namespace ToDoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly AppDbContext _context;

    public TodoController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTodos()
    {
        var todos = await _context.TodoItems
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return Ok(todos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTodo(int id)
    {
        var todo = await _context.TodoItems.FindAsync(id);

        if (todo == null)
        {
            return NotFound("Todo not found");
        }

        return Ok(todo);
    }

    [HttpPost]
    public async Task<IActionResult> AddTodo(AddDtos dto)
    {
        var todo = new TodoItem
        {
            Title = dto.Title,
            Description = dto.Description,
            IsCompleted = false,
            Priority = string.IsNullOrWhiteSpace(dto.Priority) ? "Medium" : dto.Priority,
            UserId = 1
        };

        _context.TodoItems.Add(todo);

        await _context.SaveChangesAsync();

        return Ok(todo);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTodo(int id, EditDtos dto)
    {
        var todo = await _context.TodoItems.FindAsync(id);

        if (todo == null)
        {
            return NotFound("Todo not found");
        }

        todo.Title = dto.Title;
        todo.Description = dto.Description;
        todo.IsCompleted = dto.IsCompleted;
        todo.Priority = string.IsNullOrWhiteSpace(dto.Priority) ? "Medium" : dto.Priority;

        await _context.SaveChangesAsync();

        return Ok(todo);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodo(int id)
    {
        var todo = await _context.TodoItems.FindAsync(id);

        if (todo == null)
        {
            return NotFound("Todo not found");
        }

        _context.TodoItems.Remove(todo);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            Message = "Todo Deleted Successfully"
        });
    }
}