using Microsoft.EntityFrameworkCore;
using Template.Domain.Items;

namespace Template.Persistence.Maps;

public class ItemMap : IEntityTypeConfiguration<Domain.Items.Item>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Domain.Items.Item> builder)
    {
        builder.HasKey(i => i.ItemId);
        builder.Property(i => i.ItemId).HasVogenConversion();
        builder.Property(i => i.ItemName).IsRequired().HasMaxLength(200);
    }
}