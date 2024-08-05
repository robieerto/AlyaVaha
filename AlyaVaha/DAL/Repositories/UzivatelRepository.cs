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
            var context = new AlyaVahaDbContext();
            return context.Uzivatelia.FirstOrDefault(u => u.Login == login);
        }

        public static Uzivatel? Authenticate(string login, string password)
        {
            var context = new AlyaVahaDbContext();
            return context.Uzivatelia.FirstOrDefault(u => u.Login == login && u.Heslo == password);
        }

        public static OperationResult Add(Uzivatel uzivatel)
        {
            try
            {
                var context = new AlyaVahaDbContext();
                uzivatel.DatumVytvorenia = DateTime.Now;
                uzivatel.DatumUpravy = DateTime.Now;
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
                var context = new AlyaVahaDbContext();
                var oldUzivatel = context.Uzivatelia.FirstOrDefault(x => x.Id == uzivatel.Id);
                if (oldUzivatel == null)
                {
                    return new OperationResult("Užívateľ neexistuje", false);
                }
                if (uzivatel.Id == 1)
                {
                    if (uzivatel.JeAdmin == false)
                    {
                        return new OperationResult("Admin musí mať admin oprávnenia", false);
                    }
                    if (uzivatel.Login != oldUzivatel.Login)
                    {
                        return new OperationResult("Užívateľské meno admina sa nedá zmeniť", false);
                    }
                }
                uzivatel.DatumUpravy = DateTime.Now;
                context.ChangeTracker.Clear();
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
                var context = new AlyaVahaDbContext();
                var uzivatel = context.Uzivatelia.FirstOrDefault(x => x.Id == id);
                if (uzivatel == null)
                {
                    return new OperationResult("Užívateľ neexistuje", false);
                }
                if (uzivatel.Id == 1)
                {
                    return new OperationResult("Admin sa nedá zmazať", false);
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
