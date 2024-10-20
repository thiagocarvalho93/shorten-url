using System.ComponentModel.DataAnnotations;

namespace ShorterUrl.DTOs;

public class LinkChangeAliasRequestDTO
{
    [Required(ErrorMessage = "Alias is required")]
    [StringLength(30, ErrorMessage = "Alias must be between 1 and 30 characters", MinimumLength = 1)]
    public string Alias { get; set; }
}