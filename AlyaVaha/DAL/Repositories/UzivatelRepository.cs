using AlyaVaha.Models;

namespace AlyaVaha.DAL.Repositories
{
    public static class UzivatelRepository
    {
        public static List<Uzivatel> GetList()
        {
            var context = new AlyaVahaDbContext();
            return context.Uzivatelia.ToList();
        }

        public static Uzivatel? Get(string login)
        {
            using var context = new AlyaVahaDbContext();
            return context.Uzivatelia.FirstOrDefault(u => u.Login == login);
        }

        public static void Add(Uzivatel uzivatel)
        {
            using var context = new AlyaVahaDbContext();
            context.Uzivatelia.Add(uzivatel);
            context.SaveChanges();
        }

        public static void Update(Uzivatel uzivatel)
        {
            using var context = new AlyaVahaDbContext();
            context.Uzivatelia.Update(uzivatel);
            context.SaveChanges();
        }

        public static void Delete(int id)
        {
            using var context = new AlyaVahaDbContext();
            var uzivatel = context.Uzivatelia.FirstOrDefault(x => x.Id == id);
            if (uzivatel != null)
            {
                context.Uzivatelia.Remove(uzivatel);
                context.SaveChanges();
            }
        }
    }
}
