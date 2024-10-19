using System.ComponentModel.DataAnnotations;

namespace ShorterUrl.DTOs
{
    public class ShortUrlInsertRequestDTO
    {
        [Required(ErrorMessage = "url is required")]
        public string Url { get; set; }
    }
}