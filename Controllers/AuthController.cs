using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using ToDoApi.DTOs;
using ToDoApi.Models;

namespace ToDoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDtos dto)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == dto.Email);

        if (existingUser != null)
        {
            return BadRequest("Email already exists");
        }

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            Message = "Registration Successful"
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDtos dto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == dto.Email);

        if (user == null)
        {
            return Unauthorized("Invalid Email");
        }

        bool isPasswordValid =
            BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            return Unauthorized("Invalid Password");
        }

        return Ok(new AuthResponseDtos
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Token = "dummy-token"
        });
    }
}