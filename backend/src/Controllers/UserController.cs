using Microsoft.AspNetCore.Mvc;
using ShorterUrl.Models;
using ShorterUrl.Service;

namespace ShorterUrl.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    // Request DTO > password encryption
    [HttpPost]
    public async Task<IActionResult> CreateUser(UserModel user)
    {
        await _userService.AddUserAsync(user);
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }
}