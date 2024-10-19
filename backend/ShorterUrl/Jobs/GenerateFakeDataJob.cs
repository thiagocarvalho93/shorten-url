using ShorterUrl.Repository;
using ShorterUrl.Service;

namespace ShorterUrl.Jobs;

public class GenerateFakeDataJob : IHostedService
{
    private readonly FakeDataService _fakeDataService;
    private readonly ILogger<GenerateFakeDataJob> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public GenerateFakeDataJob(FakeDataService fakeDataService, ILogger<GenerateFakeDataJob> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _fakeDataService = fakeDataService;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Inserting fake data...");

            using var scope = _serviceScopeFactory.CreateScope();
            var shortUrlRepository = scope.ServiceProvider.GetRequiredService<ShortUrlRepository>();
            var analyticsRepository = scope.ServiceProvider.GetRequiredService<AnalyticsRepository>();
            await InsertShortUrls(5000, shortUrlRepository, cancellationToken);
            await GenerateAnalytics(5000, shortUrlRepository, analyticsRepository, cancellationToken);

            _logger.LogInformation("Fake data succesfully generated!");
        }
        catch (Exception ex)
        {
            _logger.LogError("Error generating fake data: {message}\nStack trace: {stack}", ex.Message, ex.StackTrace);
        }
    }

    private async Task GenerateAnalytics(int threshold, ShortUrlRepository shortUrlRepository, AnalyticsRepository analyticsRepository, CancellationToken cancellationToken)
    {
        var count = await shortUrlRepository.CountAsync(cancellationToken);
        if (count < threshold)
        {
            _logger.LogInformation("Generating fake analytics...");
            var possibleIds = await shortUrlRepository.GetAllIds(cancellationToken);

            var fakeAnalytics = _fakeDataService.GenerateAnalyticsDAO(threshold - count, possibleIds);
            await analyticsRepository.AddAsync(fakeAnalytics, cancellationToken);

            _logger.LogInformation("Fake analytics inserted.");
        }
    }

    private async Task InsertShortUrls(int threshold, ShortUrlRepository shortUrlRepository, CancellationToken cancellationToken)
    {
        var count = await shortUrlRepository.CountAsync(cancellationToken);
        if (count < threshold)
        {
            _logger.LogInformation("Generating fake short urls...");

            var fakeUrlData = _fakeDataService.GenerateShortUrlDAO(threshold - count);
            await shortUrlRepository.AddAsync(fakeUrlData, cancellationToken);

            _logger.LogInformation("Fake short urls inserted.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}