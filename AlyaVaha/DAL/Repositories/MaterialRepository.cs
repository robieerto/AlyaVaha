using AlyaLibrary;
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

        public static OperationResult Add(Material material)
        {
            try
            {
                var context = new AlyaVahaDbContext();
                if (context.Materialy.Any(x => x.NazovMaterialu == material.NazovMaterialu))
                {
                    throw new Exception("Materiál s týmto názvom už existuje");
                }
                material.DatumVytvorenia = DateTime.Now;
                material.DatumUpravy = DateTime.Now;
                context.Materialy.Add(material);
                context.SaveChanges();
                return new OperationResult("Materiál bol pridaný", true);
            }
            catch (Exception ex)
            {
                Library.WriteLog(ex);
                return new OperationResult(ex.Message, false);
            }
        }

        public static OperationResult Update(Material material)
        {
            try
            {
                var context = new AlyaVahaDbContext();
                if (context.Materialy.Any(x => x.NazovMaterialu == material.NazovMaterialu && x.Id != material.Id))
                {
                    throw new Exception("Materiál s týmto názvom už existuje");
                }
                material.DatumUpravy = DateTime.Now;
                context.Materialy.Update(material);
                context.SaveChanges();
                return new OperationResult("Materiál bol upravený", true);
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
                var material = context.Materialy.FirstOrDefault(x => x.Id == id);
                if (material == null)
                {
                    return new OperationResult("Materiál neexistuje", false);
                }
                if (context.Navazovania.Any(x => x.MaterialId == id))
                {
                    return new OperationResult("Materiál je použitý", false);
                }
                context.Materialy.Remove(material);
                context.SaveChanges();
                return new OperationResult("Materiál bol zmazaný", true);
            }
            catch (Exception ex)
            {
                Library.WriteLog(ex);
                return new OperationResult(ex.Message, false);
            }
        }
    }
}
