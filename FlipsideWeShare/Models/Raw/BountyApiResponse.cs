using System.Text.Json.Serialization;

namespace FlipsideWeShare.Models.Raw;
public class BountyApiResponse
{
    [JsonPropertyName("bounties")]
    public RawBounty[]? Bounties { get; set; }

    public BountyApiResponse()
    {
    }
}
