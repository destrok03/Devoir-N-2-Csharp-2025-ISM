using gestionapp.Models;
using Microsoft.EntityFrameworkCore;

namespace gestionapp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Fournisseur> Fournisseurs => Set<Fournisseur>();
        public DbSet<Article> Articles => Set<Article>();
        public DbSet<Approvisionnement> Approvisionnements => Set<Approvisionnement>();
        public DbSet<ApprovisionnementLigne> ApprovisionnementLignes => Set<ApprovisionnementLigne>();
    }
}
