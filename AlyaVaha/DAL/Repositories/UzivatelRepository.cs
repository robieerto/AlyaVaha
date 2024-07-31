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

        public static Uzivatel? Authenticate(string login, string password)
        {
            using var context = new AlyaVahaDbContext();
            return context.Uzivatelia.FirstOrDefault(u => u.Login == login && u.Heslo == password);
        }

        public static OperationResult Add(Uzivatel uzivatel)
        {
            try
            {
                using var context = new AlyaVahaDbContext();
                context.Uzivatelia.Add(uzivatel);
                context.SaveChanges();
                return new OperationResult("Užívateľ bol pridaný", true);
            }
            catch (Exception ex)
            {
                return new OperationResult(ex.Message, false);
            }
        }

        public static OperationResult Update(Uzivatel uzivatel)
        {
            try
            {
                using var context = new AlyaVahaDbContext();
                context.Uzivatelia.Update(uzivatel);
                context.SaveChanges();
                return new OperationResult("Užívateľ bol upravený", true);
            }
            catch (Exception ex)
            {
                return new OperationResult(ex.Message, false);
            }
        }

        public static OperationResult Delete(int id)
        {
            try
            {
                using var context = new AlyaVahaDbContext();
                var uzivatel = context.Uzivatelia.FirstOrDefault(x => x.Id == id);
                if (uzivatel == null)
                {
                    return new OperationResult("Užívateľ neexistuje", false);
                }
                if (context.Navazovania.Any(x => x.UzivatelId == id))
                {
                    return new OperationResult("Užívateľ je použitý", false);
                }
                context.Uzivatelia.Remove(uzivatel);
                context.SaveChanges();
                return new OperationResult("Užívateľ bol zmazaný", true);
            }
            catch (Exception ex)
            {
                return new OperationResult(ex.Message, false);
            }
        }
    }
}
