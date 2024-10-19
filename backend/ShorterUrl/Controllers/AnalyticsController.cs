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

        [HttpGet("links/{linkId}")]
        public async Task<IActionResult> GetLinkAnalyticsById([FromRoute] int linkId, CancellationToken cancellationToken = default)
        {
            var result = await _analyticsService.GetByLinkId(linkId, cancellationToken);

            return Ok(result);
        }

        [HttpDelete("links/{linkId}")]
        public async Task<IActionResult> DeleteLinkAnalyticsById([FromRoute] int linkId, CancellationToken cancellationToken = default)
        {
            await _analyticsService.DeleteByLinkId(linkId, cancellationToken);

            return NoContent();
        }
    }
}