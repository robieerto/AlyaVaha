using AlyaLibrary;
using AlyaVaha.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;

namespace AlyaVaha.DAL.Repositories
{
    public static class NavazovanieRepository
    {
        public static LoadResult GetList(DataSourceLoadOptionsBase loadOptions)
        {
            if (loadOptions.Sort == null || loadOptions.Sort.Length < 1)
            {
                var sortinfo = new SortingInfo[]
                {
                    new SortingInfo() {Desc = true, Selector = "Id"},
                };
                loadOptions.Sort = sortinfo;
            }

            var context = new AlyaVahaDbContext();
            var query = context.Navazovania;
            return DataSourceLoader.Load(query, loadOptions);
        }

        public static void Add(Navazovanie navazovanie)
        {
            var context = new AlyaVahaDbContext();
            if (!context.Materialy.Any(x => x.Id == navazovanie.MaterialId))
            {
                navazovanie.MaterialId = null;
            }
            if (!context.Zasobniky.Any(x => x.Id == navazovanie.OdkialId))
            {
                navazovanie.OdkialId = null;
            }
            if (!context.Zasobniky.Any(x => x.Id == navazovanie.KamId))
            {
                navazovanie.KamId = null;
            }
            if (!context.Uzivatelia.Any(x => x.Id == navazovanie.UzivatelId))
            {
                navazovanie.UzivatelId = null;
            }
            try
            {
                context.Navazovania.Add(navazovanie);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Library.WriteLog("Nepodarilo sa pridať navažovanie do databázy");
            }
        }

        public static OperationResult Update(Navazovanie navazovanie)
        {
            try
            {
                var context = new AlyaVahaDbContext();
                context.Navazovania.Update(navazovanie);
                context.SaveChanges();
                return new OperationResult("Navažovanie bolo upravené", true);
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
                var navazovanie = context.Navazovania.FirstOrDefault(x => x.Id == id);
                if (navazovanie == null)
                {
                    return new OperationResult("Navažovanie neexistuje", false);
                }
                context.Navazovania.Remove(navazovanie);
                context.SaveChanges();
                return new OperationResult("Navažovanie bolo zmazané", true);
            }
            catch (Exception ex)
            {
                Library.WriteLog(ex);
                return new OperationResult(ex.Message, false);
            }
        }

        public static OperationResult DeleteByFilter(DataSourceLoadOptionsBase loadOptions)
        {
            try
            {
                var context = new AlyaVahaDbContext();
                var query = context.Navazovania.AsQueryable();

                // Apply the filter to get the filtered data
                var loadResult = DataSourceLoader.Load(query, loadOptions);
                var itemsToDelete = loadResult.data.Cast<Navazovanie>().ToList();

                if (itemsToDelete.Count == 0)
                {
                    return new OperationResult("Nie sú vybraté žiadne navažovnia", false);
                }

                context.Navazovania.RemoveRange(itemsToDelete);
                context.SaveChanges();

                return new OperationResult($"{itemsToDelete.Count} navažovaní bolo zmazaných", true);
            }
            catch (Exception ex)
            {
                Library.WriteLog(ex);
                return new OperationResult(ex.Message, false);
            }
        }
    }
}
