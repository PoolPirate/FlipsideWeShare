using System.Text.Json.Serialization;

namespace FlipsideWeShare.Models.Raw;
public class RawBounty
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("createdAt")]
    public DateTimeOffset CreatedAt { get; set; }
    [JsonPropertyName("startAt")]
    public DateTimeOffset? StartsAt { get; set; }
    [JsonPropertyName("endAt")]
    public DateTimeOffset? EndsAt { get; set; }
    [JsonPropertyName("hidden")]
    public bool Hidden { get; set; }
    [JsonPropertyName("hasCaptcha")]
    public bool HasCaptcha { get; set; }
    [JsonPropertyName("maxClaimCount")]
    public int MaxClaimCount { get; set; }
    [JsonPropertyName("grandPrizeAmount")]
    public double GrandPrizeAmount { get; set; }
    [JsonPropertyName("payoutAmount")]
    public double PayoutAmount { get; set; }
    [JsonPropertyName("slug")]
    public string? Slug { get; set; }
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    [JsonPropertyName("difficulty")]
    public string? Difficulty { get; set; }
    [JsonPropertyName("longDesc")]
    public string? LongDescription { get; set; }
    [JsonPropertyName("shortDesc")]
    public string? ShortDescription { get; set; }
    [JsonPropertyName("longDescHtml")]
    public string? LongDescriptionHtml { get; set; }
    [JsonPropertyName("shortDescHtml")]
    public string? ShortDescriptionHtml { get; set; }
    [JsonPropertyName("currency")]
    public RawCurrency? Currency { get; set; }

    public RawBounty()
    {
    }
}
