using Microsoft.EntityFrameworkCore;
namespace Template.Persistence;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DataContext() : this(new DbContextOptions<DataContext>())
    {
    }

    public virtual DbSet<Domain.Items.Item> Items { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }
}
