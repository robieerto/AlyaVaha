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
                    new SortingInfo() {Desc = true, Selector = "DatumStartu"},
                };
                loadOptions.Sort = sortinfo;
            }

            var context = new AlyaVahaDbContext();
            var query = context.Navazovania.OrderByDescending(x => x.DatumStartu);
            return DataSourceLoader.Load(query, loadOptions);
        }

        public static void Add(Navazovanie navazovanie)
        {
            var context = new AlyaVahaDbContext();
            try
            {
                context.Navazovania.Add(navazovanie);
                context.SaveChanges();
            }
            catch (Exception)
            {
                navazovanie.ZariadenieId = null;
                navazovanie.MaterialId = null;
                navazovanie.OdkialId = null;
                navazovanie.KamId = null;
                context.Navazovania.Add(navazovanie);
                context.SaveChanges();
            }
        }

        public static void Update(Navazovanie navazovanie)
        {
            var context = new AlyaVahaDbContext();
            context.Navazovania.Update(navazovanie);
            context.SaveChanges();
        }

        public static void Delete(int id)
        {
            var context = new AlyaVahaDbContext();
            var navazovanie = context.Navazovania.FirstOrDefault(x => x.Id == id);
            if (navazovanie != null)
            {
                context.Navazovania.Remove(navazovanie);
                context.SaveChanges();
            }
        }
    }
}
