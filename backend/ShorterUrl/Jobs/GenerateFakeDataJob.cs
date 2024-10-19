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
            var linkRepository = scope.ServiceProvider.GetRequiredService<LinkRepository>();
            var analyticsRepository = scope.ServiceProvider.GetRequiredService<ClickRepository>();
            await InsertLinks(5000, linkRepository, cancellationToken);
            await GenerateAnalytics(5000, linkRepository, analyticsRepository, cancellationToken);

            _logger.LogInformation("Fake data succesfully generated!");
        }
        catch (Exception ex)
        {
            _logger.LogError("Error generating fake data: {message}\nStack trace: {stack}", ex.Message, ex.StackTrace);
        }
    }

    private async Task GenerateAnalytics(int threshold, LinkRepository linkRepository, ClickRepository analyticsRepository, CancellationToken cancellationToken)
    {
        var count = await analyticsRepository.CountAsync(cancellationToken);
        if (count < threshold)
        {
            _logger.LogInformation("Generating fake analytics...");
            var possibleIds = await linkRepository.GetAllIdsAsync(cancellationToken);

            var fakeAnalytics = _fakeDataService.GenerateClickModel(threshold - count, possibleIds);
            await analyticsRepository.AddAsync(fakeAnalytics, cancellationToken);

            _logger.LogInformation("Fake analytics inserted.");
        }
    }

    private async Task InsertLinks(int threshold, LinkRepository linkRepository, CancellationToken cancellationToken)
    {
        var count = await linkRepository.CountAsync(cancellationToken);
        if (count < threshold)
        {
            _logger.LogInformation("Generating fake short urls...");

            var fakeUrlData = _fakeDataService.GenerateLinkModel(threshold - count);
            await linkRepository.AddAsync(fakeUrlData, cancellationToken);

            _logger.LogInformation("Fake short urls inserted.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}