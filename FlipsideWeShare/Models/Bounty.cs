using FlipsideWeShare.Models.Raw;

namespace FlipsideWeShare.Models;
public class Bounty
{
    public Guid Id { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? StartsAt { get; private set; }
    public DateTimeOffset? EndsAt { get; private set; }
    public bool Hidden { get; private set; }
    public bool HasCaptcha { get; private set; }
    public int MaxClaimCount { get; private set; }
    public string? Currency { get; private set; }
    public double GrandPrizeAmount { get; private set; }
    public double PayoutAmount { get; private set; }

    public string? Slug { get; private set; }
    public string? Title { get; private set; }
    public string? Difficulty { get; private set; }
    public string? LongDescription { get; private set; }
    public string? ShortDescription { get; private set; }

    public Bounty(RawBounty bounty)
    {
        Id = bounty.Id;
        CreatedAt = bounty.CreatedAt;
        StartsAt = bounty.StartsAt;
        EndsAt = bounty.EndsAt;
        Hidden = bounty.Hidden;
        HasCaptcha = bounty.HasCaptcha;
        MaxClaimCount = bounty.MaxClaimCount;
        Currency = bounty.Currency?.Symbol;
        GrandPrizeAmount = bounty.GrandPrizeAmount;
        PayoutAmount = bounty.PayoutAmount;
        Slug = bounty.Slug;
        Title = bounty.Title;
        Difficulty = bounty.Difficulty;
        LongDescription = String.IsNullOrWhiteSpace(bounty.LongDescription) ? bounty.LongDescriptionHtml : bounty.LongDescription;
        ShortDescription = String.IsNullOrWhiteSpace(bounty.ShortDescription) ? bounty.ShortDescriptionHtml : bounty.ShortDescription;
    }
}
