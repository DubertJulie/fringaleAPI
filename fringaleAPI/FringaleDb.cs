using fringaleAPI;
using Microsoft.EntityFrameworkCore;

class FringaleDb : DbContext
{
    public FringaleDb(DbContextOptions<FringaleDb> options)
        : base(options) { }

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Plat> Plats => Set<Plat>();
    public DbSet<Commande> Commandes => Set<Commande>();
}