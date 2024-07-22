using AlyaVaha.Models;

namespace AlyaVaha.DAL.Repositories
{
    public static class NavazovanieRepository
    {
        public static List<Navazovanie> GetList()
        {
            var context = new AlyaVahaDbContext();
            return context.Navazovania.OrderByDescending(x => x.DatumStartu).ToList();
        }

        public static void Add(Navazovanie navazovanie)
        {
            var context = new AlyaVahaDbContext();
            try
            {
                context.Navazovania.Add(navazovanie);
                context.SaveChanges();
            }
            catch (Exception)
            {
                navazovanie.ZariadenieId = null;
                navazovanie.MaterialId = null;
                navazovanie.OdkialId = null;
                navazovanie.KamId = null;
                context.Navazovania.Add(navazovanie);
                context.SaveChanges();
            }
        }

        public static void Update(Navazovanie navazovanie)
        {
            var context = new AlyaVahaDbContext();
            context.Navazovania.Update(navazovanie);
            context.SaveChanges();
        }

        public static void Delete(int id)
        {
            var context = new AlyaVahaDbContext();
            var navazovanie = context.Navazovania.FirstOrDefault(x => x.Id == id);
            if (navazovanie != null)
            {
                context.Navazovania.Remove(navazovanie);
                context.SaveChanges();
            }
        }
    }
}
