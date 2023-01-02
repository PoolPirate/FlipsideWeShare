using FlipsideWeShare.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FlipsideWeShare.Database;
public class BountyContext : DbContext
{
    public DbSet<BountyReference> Bounties { get; set; } = null!;

    public BountyContext(DbContextOptions options) 
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) 
        => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
}
