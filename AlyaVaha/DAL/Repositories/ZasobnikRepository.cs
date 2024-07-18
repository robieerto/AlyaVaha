using AlyaVaha.Models;

namespace AlyaVaha.DAL.Repositories
{
    public static class ZasobnikRepository
    {
        public static List<Zasobnik> GetList()
        {
            var context = new AlyaVahaDbContext();
            return context.Zasobniky.OrderByDescending(x => x.DatumUpravy).ToList();
        }

        public static void Add(Zasobnik zasobnik)
        {
            var context = new AlyaVahaDbContext();
            zasobnik.DatumVytvorenia = DateTime.Now;
            zasobnik.DatumUpravy = DateTime.Now;
            context.Zasobniky.Add(zasobnik);
            context.SaveChanges();
        }

        public static void Update(Zasobnik zasobnik)
        {
            var context = new AlyaVahaDbContext();
            zasobnik.DatumUpravy = DateTime.Now;
            context.Zasobniky.Update(zasobnik);
            context.SaveChanges();
        }

        public static void Delete(int id)
        {
            var context = new AlyaVahaDbContext();
            var zasobnik = context.Zasobniky.FirstOrDefault(x => x.Id == id);
            if (zasobnik != null)
            {
                context.Zasobniky.Remove(zasobnik);
                context.SaveChanges();
            }
        }
    }
}
