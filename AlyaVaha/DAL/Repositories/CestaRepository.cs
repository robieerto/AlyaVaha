using AlyaVaha.Models;

namespace AlyaVaha.DAL.Repositories
{
    public static class CestaRepository
    {
        public static List<Cesta> GetList()
        {
            var context = new AlyaVahaDbContext();
            return context.Cesty.ToList();
        }

        public static void Add(Cesta cesta)
        {
            var context = new AlyaVahaDbContext();
            context.Cesty.Add(cesta);
            context.SaveChanges();
        }

        public static void Update(Cesta cesta)
        {
            var context = new AlyaVahaDbContext();
            context.Cesty.Update(cesta);
            context.SaveChanges();
        }

        public static void Delete(int id)
        {
            var context = new AlyaVahaDbContext();
            var cesta = context.Cesty.FirstOrDefault(x => x.Id == id);
            if (cesta != null)
            {
                context.Cesty.Remove(cesta);
                context.SaveChanges();
            }
        }
    }
}
