using Microsoft.EntityFrameworkCore;
using fringaleAPI;

class FringaleAPIDb : DbContext
{
    public FringaleAPIDb(DbContextOptions<FringaleAPIDb> options)
        : base(options) { }

    public DbSet<Plat> Plats => Set<Plat>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Commande> Commandes => Set<Commande>();
}