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
            var zariadenia = new List<Models.Zariadenie>
            {
                new Models.Zariadenie { Id = 1, NazovZariadenia = "Váha 1", IpAdresa = "192.168.1.10", Port = 3396 }
            };

            modelBuilder.Entity<Models.Zariadenie>().HasData(zariadenia);

            base.OnModelCreating(modelBuilder);
        }
    }
}
