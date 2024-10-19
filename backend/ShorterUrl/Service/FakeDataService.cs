using Bogus;
using ShorterUrl.Models;

namespace ShorterUrl.Service;

public class FakeDataService
{
    private readonly Faker<LinkModel> linkFake;
    private readonly Faker<AnalyticsModel> analyticsFake;

    public FakeDataService()
    {
        linkFake = SetupLinkFake();
        analyticsFake = new Faker<AnalyticsModel>()
            .RuleFor(x => x.ClickDate, f => f.Date.Recent())
            .RuleFor(x => x.UserAgent, f => f.Internet.UserAgent())
            .RuleFor(x => x.IpAdress, f => f.Internet.Ip())
            .RuleFor(x => x.Referrer, f => f.Internet.Url())
            .RuleFor(x => x.Location, x => x.Address.Country());
    }

    public IEnumerable<LinkModel> GenerateLinkDAO(int Length)
    {
        return linkFake.Generate(Length);
    }

    public IEnumerable<AnalyticsModel> GenerateAnalyticsDAO(int Length, int[] possibleLinkIds)
    {
        analyticsFake.RuleFor(x => x.LinkId, f => f.PickRandomParam(possibleLinkIds));
        return analyticsFake.Generate(Length);
    }

    private static Faker<LinkModel> SetupLinkFake()
    {
        return new Faker<LinkModel>()
                    .RuleFor(x => x.CreatedAt, f => f.Date.Recent())
                    .RuleFor(x => x.ExpiresAt, f => f.Date.Soon())
                    .RuleFor(x => x.OriginalUrl, f => f.Internet.Url())
                    .RuleFor(x => x.ShortCode, f => f.Random.AlphaNumeric(5));
    }
}