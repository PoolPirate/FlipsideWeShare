using Common.Services;
using FlipsideWeShare.Configuration;
using FlipsideWeShare.Database;
using FlipsideWeShare.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;

namespace FlipsideWeShare.Services;
[ServiceLevel(1)]
public class WeShareClient : Singleton
{
    private readonly HttpClient Client;
    [Inject]
    private readonly ShareOptions ShareOptions = null!;
    [Inject]
    private readonly IDbContextFactory<BountyContext> DbContextFactory = null!;

    public WeShareClient()
    {
        Client = new HttpClient();
    }

    protected override ValueTask InitializeAsync()
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Tls13;
        return default;
    }

    public async Task PublishBountyAsync(Bounty bounty)
    {
        using var dbContext = await DbContextFactory.CreateDbContextAsync();
        using var transaction = await dbContext.Database.BeginTransactionAsync();

        var bountyReference = new BountyReference(bounty.Id);
        dbContext.Bounties.Add(bountyReference);
        await dbContext.SaveChangesAsync();

        if (!await PublishPostAsync(bounty))
        {
            await transaction.RollbackAsync();
            return;
        }

        Logger.LogInformation("Posted bounty to WeShare: Id={bountyId}", bounty.Id);
        await transaction.CommitAsync();
    }

    private async Task<bool> PublishPostAsync(Bounty bounty)
    {
        var bountyUrl = new Uri($"https://flipsidecrypto.xyz/drops/{bounty.Slug}");

        var rawContent = new Dictionary<string, object>()
        {
            ["bountyUrl"] = bountyUrl.ToString(),
            ["bounty"] = bounty
        };

        var content = JsonContent.Create(rawContent);

        try
        {
            var response = await Client.PostAsync($"https://we-share-live.de/Api/Post-Management/{ShareOptions.Secret}", content);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogCritical("Failed submitting new Bounty to WeShare! Status={statusCode}", response.StatusCode);
            }

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Logger.LogCritical(ex, "Failed submitting new Bounty to WeShare!");
            return false;
        }
    }
}
