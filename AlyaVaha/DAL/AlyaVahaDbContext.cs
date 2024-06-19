using Microsoft.EntityFrameworkCore;

namespace AlyaVaha.DAL
{
    public class AlyaVahaDbContext : DbContext
    {
        public DbSet<Models.Zariadenie> Zariadenia { get; set; }
        public DbSet<Models.Uzivatel> Uzivatelia { get; set; }
        public DbSet<Models.Material> Materialy { get; set; }
        public DbSet<Models.Zasobnik> Zasobniky { get; set; }
        public DbSet<Models.Cesta> Cesty { get; set; }
        public DbSet<Models.Navazovanie> Navazovania { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(LocalDB)\\MSSQLLocalDB;Database=AlyaVaha;MultipleActiveResultSets=True;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
