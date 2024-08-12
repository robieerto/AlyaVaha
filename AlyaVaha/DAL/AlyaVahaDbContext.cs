using Microsoft.EntityFrameworkCore;

namespace AlyaVaha.DAL
{
    public class AlyaVahaDbContext : DbContext
    {
        public DbSet<Models.Program> Programy { get; set; }
        public DbSet<Models.Zariadenie> Zariadenia { get; set; }
        public DbSet<Models.Uzivatel> Uzivatelia { get; set; }
        public DbSet<Models.Material> Materialy { get; set; }
        public DbSet<Models.Zasobnik> Zasobniky { get; set; }
        public DbSet<Models.Navazovanie> Navazovania { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(LocalDB)\\AlyaDB;Database=AlyaVaha;MultipleActiveResultSets=True;User ID=sa;Password=MuL7J58B6ftSWkaESXLN");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var programy = new List<Models.Program>
            {
                new Models.Program { Id = 1 }
            };

            var uzivatelia = new List<Models.Uzivatel>
            {
                new Models.Uzivatel { Id = 1, Login = "Admin", Heslo = "alya123456", JeAdmin = true, JeAktivny = true, DatumVytvorenia = DateTime.Now, DatumUpravy = DateTime.Now, },
                new Models.Uzivatel { Id = 2, Login = "Obsluha", Heslo = "obsluha123", JeAdmin = false, JeAktivny = true, DatumVytvorenia = DateTime.Now, DatumUpravy = DateTime.Now, }
            };

            var zariadenia = new List<Models.Zariadenie>
            {
                new Models.Zariadenie { Id = 1, NazovZariadenia = "Váha 1", IpAdresa = "192.168.1.10", Port = 3396 }
            };

            var materialy = new List<Models.Material>
            {
                new Models.Material { Id = 1, NazovMaterialu = "Materiál 1", DatumVytvorenia = DateTime.Now, DatumUpravy = DateTime.Now, HmotnostMaterialu = 100, JeAktivny = true },
                new Models.Material { Id = 2, NazovMaterialu = "Materiál 2", DatumVytvorenia = DateTime.Now, DatumUpravy = DateTime.Now, HmotnostMaterialu = 200, JeAktivny = true },
            };

            var zasobniky = new List<Models.Zasobnik>
            {
                new Models.Zasobnik { Id = 1, NazovZasobnika = "Zásobník 1", Skratka = "Z1", DatumVytvorenia = DateTime.Now, DatumUpravy = DateTime.Now, CestaDoVahy = true, CestaZVahy = true},
                new Models.Zasobnik { Id = 2, NazovZasobnika = "Zásobník 2", Skratka = "Z2", DatumVytvorenia = DateTime.Now, DatumUpravy = DateTime.Now, CestaDoVahy = true, CestaZVahy = true},
                new Models.Zasobnik { Id = 3, NazovZasobnika = "Zásobník 3", Skratka = "Z3", DatumVytvorenia = DateTime.Now, DatumUpravy = DateTime.Now, CestaDoVahy = true, CestaZVahy = true},
            };

            modelBuilder.Entity<Models.Program>().HasData(programy);
            modelBuilder.Entity<Models.Uzivatel>().HasData(uzivatelia);
            modelBuilder.Entity<Models.Zariadenie>().HasData(zariadenia);
            modelBuilder.Entity<Models.Material>().HasData(materialy);
            modelBuilder.Entity<Models.Zasobnik>().HasData(zasobniky);

            base.OnModelCreating(modelBuilder);
        }
    }
}
