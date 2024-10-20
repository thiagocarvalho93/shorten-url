using System.ComponentModel.DataAnnotations;

namespace ShorterUrl.DTOs;

public sealed record LoginRequestDTO
{
    [Required(ErrorMessage = "username is required")]
    public string Username { get; set; }
    [Required(ErrorMessage = "password is required")]
    public string Password { get; set; }
}