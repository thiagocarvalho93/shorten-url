using System.ComponentModel.DataAnnotations;

namespace ShorterUrl.DTOs;

public sealed record LinkInsertRequestDTO
{
    [Required(ErrorMessage = "url is required")]
    public string Url { get; set; } = "";
}