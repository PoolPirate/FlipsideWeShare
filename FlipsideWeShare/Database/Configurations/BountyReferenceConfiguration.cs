using FlipsideWeShare.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlipsideWeShare.Database.Configurations;
internal class BountyReferenceConfiguration : IEntityTypeConfiguration<BountyReference>
{
    public void Configure(EntityTypeBuilder<BountyReference> builder)
    {
        builder.ToTable("Bounties");

        builder.Property(x => x.Id);
        builder.HasKey(x => x.Id);
    }
}
