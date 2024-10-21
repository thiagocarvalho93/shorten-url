using Microsoft.AspNetCore.Mvc;
using ShorterUrl.Service;

namespace ShorterUrl.Controllers
{
    [ApiController]
    [Route("analytics")]
    public class AnalyticsController : ControllerBase
    {
        private readonly AnalyticsService _analyticsService;

        public AnalyticsController(AnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpGet("general")]
        public async Task<IActionResult> GetGeneralAnalytics(CancellationToken cancellationToken = default)
        {
            var result = await _analyticsService.GetGeneralAnalytics(cancellationToken);

            return Ok(result);
        }

        [HttpGet("links/{shortCode}")]
        public async Task<IActionResult> GetLinkAnalyticsByShortCode([FromRoute] string shortCode, CancellationToken cancellationToken = default)
        {
            var result = await _analyticsService.GetByLinkShortUrl(shortCode, cancellationToken);

            return Ok(result);
        }

        [HttpGet("links/{shortCode}/locations")]
        public async Task<IActionResult> GetClickLocationsByLinkShortUrl([FromRoute] string shortCode, CancellationToken cancellationToken = default)
        {
            var result = await _analyticsService.GetClickLocationsByLinkShortUrl(shortCode, cancellationToken);

            return Ok(result);
        }

        [HttpGet("links/{shortCode}/device-languages")]
        public async Task<IActionResult> GetClickDeviceLanguageByLinkShortUrl([FromRoute] string shortCode, CancellationToken cancellationToken = default)
        {
            var result = await _analyticsService.GetClickDeviceLanguageByLinkShortUrl(shortCode, cancellationToken);

            return Ok(result);
        }

        [HttpDelete("links/{shortCode}")]
        public async Task<IActionResult> DeleteLinkClicksByShortCode([FromRoute] string shortCode, CancellationToken cancellationToken = default)
        {
            await _analyticsService.DeleteByLinkShortCode(shortCode, cancellationToken);

            return NoContent();
        }
    }
}