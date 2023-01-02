using Common.Services;
using FlipsideWeShare.Database;
using FlipsideWeShare.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlipsideWeShare.Services;
[ServiceLevel(2)]
public class ScrapingService : Singleton
{
    [Inject]
    private readonly FlipSideClient FlipSideClient;
    [Inject]
    private readonly WeShareClient WeShareClient;
    [Inject]
    private readonly IDbContextFactory<BountyContext> DbContextFactory;

    protected override async ValueTask RunAsync()
    {
        while (true)
        {
            try
            {
                await ScrapeBountiesAsync();
            }
            catch { }
            finally
            {
                await Task.Delay(20000);
            }
        }
    }

    private async Task ScrapeBountiesAsync()
    {
        var bounties = await FlipSideClient.GetActiveBountiesAsync();
        var filteredBounties = await GetUnpostedBountiesAsync(bounties);

        Logger.LogInformation("Found {count} unposted bounties!", filteredBounties.Count);

        foreach (var bounty in filteredBounties)
        {
            await WeShareClient.PublishBountyAsync(bounty);
        }
    }

    private async Task<IList<Bounty>> GetUnpostedBountiesAsync(IList<Bounty> bounties)
    {
        using var dbContext = await DbContextFactory.CreateDbContextAsync();

        var filteredBounties = new List<Bounty>();

        foreach(var bounty in bounties)
        {
            if (await dbContext.Bounties.AnyAsync(x => x.Id == bounty.Id))
            {
                continue;
            }

            filteredBounties.Add(bounty);
        }

        return filteredBounties;
    }
}
