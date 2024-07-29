using AlyaVaha.Models;

namespace AlyaVaha.DAL.Repositories
{
    public static class ZariadenieRepository
    {
        public static List<Zariadenie> GetList()
        {
            var context = new AlyaVahaDbContext();
            return context.Zariadenia.ToList();
        }

        public static void Add(Zariadenie zariadenie)
        {
            var context = new AlyaVahaDbContext();
            context.Zariadenia.Add(zariadenie);
            context.SaveChanges();
        }

        public static void Update(Zariadenie zariadenie)
        {
            var context = new AlyaVahaDbContext();
            var updatedZariadenie = context.Zariadenia.FirstOrDefault(x => x.Id == zariadenie.Id);
            if (updatedZariadenie != null)
            {
                updatedZariadenie.NazovZariadenia = zariadenie.NazovZariadenia;
                updatedZariadenie.IpAdresa = zariadenie.IpAdresa;
                updatedZariadenie.Port = zariadenie.Port;
                context.Zariadenia.Update(updatedZariadenie);
                context.SaveChanges();
            }
        }

        public static void UpdateStatistiky(int id, double? navazeneMnozstvo, int? navazenyPocetDavok)
        {
            var context = new AlyaVahaDbContext();
            var updatedZariadenie = context.Zariadenia.FirstOrDefault(x => x.Id == id);
            if (updatedZariadenie != null)
            {
                updatedZariadenie.PocetNavazeni++;
                if (navazeneMnozstvo.HasValue)
                {
                    updatedZariadenie.NavazeneMnozstvo += navazeneMnozstvo.Value;
                }
                if (navazenyPocetDavok.HasValue)
                {
                    updatedZariadenie.NavazenyPocetDavok += navazenyPocetDavok.Value;
                }
                context.Zariadenia.Update(updatedZariadenie);
                context.SaveChanges();
            }
        }

        public static void Delete(int id)
        {
            var context = new AlyaVahaDbContext();
            var zariadenie = context.Zariadenia.FirstOrDefault(x => x.Id == id);
            if (zariadenie != null)
            {
                context.Zariadenia.Remove(zariadenie);
                context.SaveChanges();
            }
        }
    }
}
