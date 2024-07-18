using AlyaVaha.Models;

namespace AlyaVaha.DAL.Repositories
{
    public static class MaterialRepository
    {
        public static List<Material> GetList()
        {
            var context = new AlyaVahaDbContext();
            return context.Materialy.OrderByDescending(x => x.DatumUpravy).ToList();
        }

        public static void Add(Material material)
        {
            var context = new AlyaVahaDbContext();
            material.DatumVytvorenia = DateTime.Now;
            material.DatumUpravy = DateTime.Now;
            context.Materialy.Add(material);
            context.SaveChanges();
        }

        public static void Update(Material material)
        {
            var context = new AlyaVahaDbContext();
            material.DatumUpravy = DateTime.Now;
            context.Materialy.Update(material);
            context.SaveChanges();
        }

        public static void Delete(int id)
        {
            var context = new AlyaVahaDbContext();
            var material = context.Materialy.FirstOrDefault(x => x.Id == id);
            if (material != null)
            {
                context.Materialy.Remove(material);
                context.SaveChanges();
            }
        }
    }
}
