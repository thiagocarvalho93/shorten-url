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
            var data = _fakeDataService.GenerateShortUrlDAO(1000);

            using var scope = _serviceScopeFactory.CreateScope();
            var shortUrlRepository = scope.ServiceProvider.GetRequiredService<ShortUrlRepository>();
            await shortUrlRepository.AddAsync(data, cancellationToken);

            _logger.LogInformation("Fake data succesfully generated!");
        }
        catch (Exception ex)
        {
            _logger.LogError("Error generating fake data: {message}\nStack trace: {stack}", ex.Message, ex.StackTrace);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}