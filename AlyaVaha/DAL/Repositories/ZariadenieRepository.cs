using AlyaLibrary;
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

        public static OperationResult Add(Zariadenie zariadenie)
        {
            try
            {
                var context = new AlyaVahaDbContext();
                if (context.Zariadenia.Any(x => x.NazovZariadenia == zariadenie.NazovZariadenia))
                {
                    throw new Exception("Zariadenie s týmto názvom už existuje");
                }
                context.Zariadenia.Add(zariadenie);
                context.SaveChanges();
                return new OperationResult("Zariadenie bolo pridané", true);
            }
            catch (Exception ex)
            {
                Library.WriteLog(ex);
                return new OperationResult(ex.Message, false);
            }
        }

        public static OperationResult Update(Zariadenie zariadenie)
        {
            try
            {
                var context = new AlyaVahaDbContext();
                if (context.Zariadenia.Any(x => x.NazovZariadenia == zariadenie.NazovZariadenia && x.Id != zariadenie.Id))
                {
                    throw new Exception("Zariadenie s týmto názvom už existuje");
                }
                var updatedZariadenie = context.Zariadenia.FirstOrDefault(x => x.Id == zariadenie.Id);
                if (updatedZariadenie == null)
                {
                    return new OperationResult("Zariadenie neexistuje", false);
                }
                updatedZariadenie.NazovZariadenia = zariadenie.NazovZariadenia;
                updatedZariadenie.IpAdresa = zariadenie.IpAdresa;
                updatedZariadenie.Port = zariadenie.Port;
                context.Zariadenia.Update(updatedZariadenie);
                context.SaveChanges();
                return new OperationResult("Zariadenie bolo upravené", true);
            }
            catch (Exception ex)
            {
                Library.WriteLog(ex);
                return new OperationResult(ex.Message, false);
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

        public static OperationResult Delete(int id)
        {
            try
            {
                var context = new AlyaVahaDbContext();
                var zariadenie = context.Zariadenia.FirstOrDefault(x => x.Id == id);
                if (zariadenie == null)
                {
                    return new OperationResult("Zariadenie neexistuje", false);
                }
                if (context.Navazovania.Any(x => x.ZariadenieId == id))
                {
                    return new OperationResult("Zariadenie je použité", false);
                }
                context.Zariadenia.Remove(zariadenie);
                context.SaveChanges();
                return new OperationResult("Zariadenie bolo vymazané", true);
            }
            catch (Exception ex)
            {
                Library.WriteLog(ex);
                return new OperationResult(ex.Message, false);
            }
        }
    }
}
