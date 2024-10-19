using Bogus;
using ShorterUrl.Models;

namespace ShorterUrl.Service;

public class FakeDataService
{
    private readonly Faker<ShortUrlDAO> shortUrlFake;
    private readonly Faker<AnalyticsDAO> analyticsFake;

    public FakeDataService()
    {
        shortUrlFake = SetupShortUrlFake();
        analyticsFake = new Faker<AnalyticsDAO>()
            .RuleFor(x => x.ClickDate, f => f.Date.Recent())
            .RuleFor(x => x.UserAgent, f => f.Internet.UserAgent())
            .RuleFor(x => x.IpAdress, f => f.Internet.Ip())
            .RuleFor(x => x.Referrer, f => f.Internet.Url())
            .RuleFor(x => x.Location, x => x.Address.Country());
    }

    public IEnumerable<ShortUrlDAO> GenerateShortUrlDAO(int Length)
    {
        return shortUrlFake.Generate(Length);
    }

    public IEnumerable<AnalyticsDAO> GenerateAnalyticsDAO(int Length, int[] possibleShortUrlIds)
    {
        analyticsFake.RuleFor(x => x.ShortUrlId, f => f.PickRandomParam(possibleShortUrlIds));
        return analyticsFake.Generate(Length);
    }

    private static Faker<ShortUrlDAO> SetupShortUrlFake()
    {
        return new Faker<ShortUrlDAO>()
                    .RuleFor(x => x.CreatedAt, f => f.Date.Recent())
                    .RuleFor(x => x.ExpiresAt, f => f.Date.Soon())
                    .RuleFor(x => x.OriginalUrl, f => f.Internet.Url())
                    .RuleFor(x => x.ShortCode, f => f.Random.AlphaNumeric(5));
    }
}