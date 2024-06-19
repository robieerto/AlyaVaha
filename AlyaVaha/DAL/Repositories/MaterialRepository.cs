using AlyaVaha.Models;

namespace AlyaVaha.DAL.Repositories
{
    public static class MaterialRepository
    {
        public static List<Material> GetList()
        {
            var context = new AlyaVahaDbContext();
            return context.Materialy.ToList();
        }

        public static void Add(Material material)
        {
            var context = new AlyaVahaDbContext();
            context.Materialy.Add(material);
            context.SaveChanges();
        }

        public static void Update(Material material)
        {
            var context = new AlyaVahaDbContext();
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
