using System.ComponentModel.DataAnnotations;

namespace ShorterUrl.DTOs;

public sealed record ShortUrlInsertRequestDTO
{
    [Required(ErrorMessage = "url is required")]
    public string Url { get; set; } = "";
}