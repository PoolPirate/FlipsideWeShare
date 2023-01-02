using Common.Services;
using FlipsideWeShare.Models;
using FlipsideWeShare.Models.Raw;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace FlipsideWeShare.Services;
[ServiceLevel(1)]
public class FlipSideClient : Singleton
{
    private const string FlipSideEarnUrl = "https://flipsidecrypto.xyz/earn";
    private static string FlipSideBountyUrl(string id) => $"https://flipsidecrypto.xyz/drops/{id}";
    private static string FlipSideApiUrl = "https://flipsidecrypto.xyz/earn?_data=routes%2Fearn%2Findex";

    [Inject]
    private readonly HttpClient Client = null!;

    protected override ValueTask InitializeAsync()
    {
        Client.DefaultRequestHeaders.UserAgent.Clear();
        Client.DefaultRequestHeaders.UserAgent.ParseAdd("WeShare.de Webscraper/1.0");
        return default;
    }

    public async Task<IList<Bounty>> GetActiveBountiesAsync()
    {
        try
        {
            var apiResponse = await Client.GetFromJsonAsync<BountyApiResponse>(FlipSideApiUrl);

            return apiResponse is null || apiResponse.Bounties is null
                ? throw new Exception("Empty Response from Flipside")
                : apiResponse.Bounties.Select(x => new Bounty(x)).ToList();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed retrieving Bounty API endpoint!");
            return Array.Empty<Bounty>();
        }
    }

    private async Task<HtmlDocument?> GetEarnPageAsync()
    {
        try
        {
            string rawHtml = await Client.GetStringAsync(FlipSideEarnUrl);

            if (String.IsNullOrEmpty(rawHtml))
            {
                return null;
            }

            var document = new HtmlDocument();
            document.LoadHtml(rawHtml);
            return document;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed retrieving Flipside Earn Page!");
            return null;
        }
    }
    private async Task<HtmlDocument?> GetBountyPageAsync(string bountyId)
    {
        try
        {
            string rawHtml = await Client.GetStringAsync(FlipSideBountyUrl(bountyId));

            if (String.IsNullOrEmpty(rawHtml))
            {
                return null;
            }

            var document = new HtmlDocument();
            document.LoadHtml(rawHtml);
            return document;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed retrieving Flipside Bounty Page!");
            return null;
        }
    }

    private IList<BountyReference> ExtractBountyReferences(HtmlNode subPage)
    {
        string? dataId = subPage.GetAttributeValue<string?>("data-test-id", null);
        string? href = subPage.GetAttributeValue<string?>("href", null);

        var bounties = new List<BountyReference>();

        foreach(var child in subPage.ChildNodes)
        {
            if (child is null)
            {
                continue;
            }

            bounties.AddRange(ExtractBountyReferences(child));
        }

        if (dataId == "drop-item" && href is not null)
        {
            string id = href.Split('/').Last();
            bounties.Add(new BountyReference(Guid.Parse(id)));
        }

        return bounties;
    }
}
 