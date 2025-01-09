using fringaleAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

public class FringaleAPIDb : DbContext
{
    public FringaleAPIDb(DbContextOptions<FringaleAPIDb> options)
        : base(options) { }

    public DbSet<Plat> Plats => Set<Plat>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Commande> Commandes => Set<Commande>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }
}
