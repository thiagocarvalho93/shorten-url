using System.ComponentModel.DataAnnotations;

namespace ShorterUrl.DTOs
{
    public class AddShortUrlDTO
    {
        [Required(ErrorMessage = "url is required")]
        public string Url { get; set; }
    }
}