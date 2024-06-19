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
            context.Zariadenia.Update(zariadenie);
            context.SaveChanges();
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
