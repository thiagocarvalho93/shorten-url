using Bogus;
using ShorterUrl.Models;

namespace ShorterUrl.Service;

public class FakeDataService
{
    private readonly Faker<ShortUrlDAO> shortUrlFake;

    public FakeDataService()
    {
        Randomizer.Seed = new Random(123);
        shortUrlFake = SetupShortUrlFake();
    }

    public IEnumerable<ShortUrlDAO> GenerateShortUrlDAO(int Length)
    {
        return shortUrlFake.Generate(Length);
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