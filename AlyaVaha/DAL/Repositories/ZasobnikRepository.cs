using AlyaLibrary;
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

        public static OperationResult Add(Zasobnik zasobnik)
        {
            try
            {
                var context = new AlyaVahaDbContext();
                if (context.Zasobniky.Any(x => x.NazovZasobnika == zasobnik.NazovZasobnika))
                {
                    throw new Exception("Zásobník s týmto názvom už existuje");
                }
                zasobnik.DatumVytvorenia = DateTime.Now;
                zasobnik.DatumUpravy = DateTime.Now;
                context.Zasobniky.Add(zasobnik);
                context.SaveChanges();
                return new OperationResult("Zásobník bol pridaný", true);
            }
            catch (Exception ex)
            {
                Library.WriteLog(ex);
                return new OperationResult(ex.Message, false);
            }
        }

        public static OperationResult Update(Zasobnik zasobnik)
        {
            try
            {
                var context = new AlyaVahaDbContext();
                if (context.Zasobniky.Any(x => x.NazovZasobnika == zasobnik.NazovZasobnika && x.Id != zasobnik.Id))
                {
                    throw new Exception("Zásobník s týmto názvom už existuje");
                }
                zasobnik.DatumUpravy = DateTime.Now;
                context.Zasobniky.Update(zasobnik);
                context.SaveChanges();
                return new OperationResult("Zásobník bol upravený", true);
            }
            catch (Exception ex)
            {
                Library.WriteLog(ex);
                return new OperationResult(ex.Message, false);
            }
        }

        public static OperationResult Delete(int id)
        {
            try
            {
                var context = new AlyaVahaDbContext();
                var zasobnik = context.Zasobniky.FirstOrDefault(x => x.Id == id);
                if (zasobnik == null)
                {
                    return new OperationResult("Zásobník neexistuje", false);
                }
                if (context.Navazovania.Any(x => x.OdkialId == id || x.KamId == id))
                {
                    return new OperationResult("Zásobník je použitý", false);
                }
                context.Zasobniky.Remove(zasobnik);
                context.SaveChanges();
                return new OperationResult("Zásobník bol zmazaný", true);
            }
            catch (Exception ex)
            {
                Library.WriteLog(ex);
                return new OperationResult(ex.Message, false);
            }
        }
    }
}
