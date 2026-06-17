using BCrypt.Net;
using LogisticsAPI.Data;
using LogisticsAPI.DTOs;
using LogisticsAPI.Models;
using LogisticsAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LogisticsAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly LogisticsDbContext _context;
    private readonly JwtService _jwtService;

    public AuthController(
        LogisticsDbContext context,
        JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    // Register a new regular user (role = "User").
  
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (existingUser != null)
            return BadRequest(new { message = "Email already exists" });

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = "User"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok(new { message = "User registered successfully" });
    }

// Register a new admin (role = "Admin"). Requires an existing Admin JWT.

    [HttpPost("register-admin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RegisterAdmin(RegisterDto dto)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (existingUser != null)
            return BadRequest(new { message = "Email already exists" });

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = "Admin"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Admin registered successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null)
            return Unauthorized(new { message = "Invalid email or password" });

        bool validPassword =
            BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

        if (!validPassword)
            return Unauthorized(new { message = "Invalid email or password" });

        var token = _jwtService.GenerateToken(user);

       return Ok(new
{
    token,
    role = user.Role,
    name = user.Name,
    userId = user.Id
});
    }
   [HttpPost("seed-admin")]
public async Task<IActionResult> SeedAdmin()
{
    var existingAdmin = await _context.Users
        .FirstOrDefaultAsync(u => u.Role == "Admin");

    if (existingAdmin != null)
    {
        // Reset the password instead of blocking
        existingAdmin.PasswordHash = BCrypt.Net.BCrypt.HashPassword("H&RAdmin123");
        await _context.SaveChangesAsync();
        return Ok(new { message = "Admin password reset successfully" });
    }

    var user = new User
    {
        Name = "H&RAdmin",
        Email = "H&RAdmin@gmail.com",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("H&RAdmin123"),
        Role = "Admin"
    };

    _context.Users.Add(user);
    await _context.SaveChangesAsync();
    return Ok(new { message = "Admin seeded successfully" });
}
[HttpGet("debug-admin")]
public async Task<IActionResult> DebugAdmin()
{
    var admin = await _context.Users
        .Where(u => u.Role == "Admin")
        .Select(u => new { u.Id, u.Email, u.Name, u.Role })
        .FirstOrDefaultAsync();

    return Ok(admin);
}
}
