using System.ComponentModel.DataAnnotations;

namespace ShorterUrl.DTOs;

public sealed record LinkInsertRequestDTO
{
    [Required(ErrorMessage = "Url is required")]
    [Url(ErrorMessage = "Invalid URL format")]
    public string Url { get; set; } = "";
}