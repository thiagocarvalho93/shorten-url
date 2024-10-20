using Bogus;
using ShorterUrl.Models;

namespace ShorterUrl.Service;

public class FakeDataService
{
    private readonly Faker<LinkModel> linkFake;
    private readonly Faker<ClickModel> clickFake;
    private readonly Faker<UserModel> userFake;

    public FakeDataService()
    {
        linkFake = SetupLinkFake();
        clickFake = SetupClickFake();
        userFake = SetupUserFake();
    }

    public IEnumerable<LinkModel> GenerateLinkModel(int Length)
    {
        return linkFake.Generate(Length);
    }

    public IEnumerable<ClickModel> GenerateClickModel(int Length, int[] possibleLinkIds)
    {
        clickFake.RuleFor(x => x.LinkId, f => f.PickRandomParam(possibleLinkIds));
        return clickFake.Generate(Length);
    }

    private static Faker<LinkModel> SetupLinkFake()
    {
        return new Faker<LinkModel>()
            .RuleFor(x => x.CreatedAt, f => f.Date.Recent())
            .RuleFor(x => x.ExpiresAt, f => f.Date.Soon())
            .RuleFor(x => x.OriginalUrl, f => f.Internet.Url())
            .RuleFor(x => x.ShortCode, f => f.Random.AlphaNumeric(7));
    }

    private static Faker<ClickModel> SetupClickFake()
    {
        return new Faker<ClickModel>()
            .RuleFor(x => x.ClickDate, f => f.Date.Recent())
            .RuleFor(x => x.UserAgent, f => f.Internet.UserAgent())
            .RuleFor(x => x.IpAdress, f => f.Internet.Ip())
            .RuleFor(x => x.Referrer, f => f.Internet.Url())
            .RuleFor(x => x.Location, x => x.Address.Country());
    }

    private static Faker<UserModel> SetupUserFake()
    {
        return new Faker<UserModel>()
            .RuleFor(x => x.DateCreated, f => f.Date.Recent())
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .RuleFor(x => x.FirstName, f => f.Person.FirstName)
            .RuleFor(x => x.LastName, f => f.Person.LastName)
            .RuleFor(x => x.IsActive, f => f.Random.Bool())
            .RuleFor(x => x.Username, f => f.Internet.UserName())
            .RuleFor(x => x.PasswordHash, f => f.Random.Hash());
    }
}