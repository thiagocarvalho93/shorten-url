using System.ComponentModel.DataAnnotations;

namespace ShorterUrl.DTOs;

public class PaginatedRequestDTO
{
    [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than or equal to 1")]
    public int Page { get; set; } = 1;

    [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100")]
    public int PageSize { get; set; } = 10;
}