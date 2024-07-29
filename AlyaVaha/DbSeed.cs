using AlyaVaha.DAL;

namespace AlyaVaha
{
    public static class DbSeed
    {
        public static void Seed()
        {
            var context = new AlyaVahaDbContext();
            var navazovania = new List<Models.Navazovanie>();
            int i = 0;
            for (; i < 5000000; i++)
            {
                navazovania.Add(new Models.Navazovanie
                {
                    ZariadenieId = 1,
                    DatumStartu = DateTime.Now,
                    CasStartu = DateTime.Now.ToString("HH:mm"),
                    NavazeneMnozstvo = i,
                    OdkialId = 1,
                    KamId = 2,
                    MaterialId = 1,
                });
                if (i % 100000 == 0)
                {
                    context.Navazovania.AddRange(navazovania);
                    context.SaveChanges();
                    navazovania.Clear();
                    Console.WriteLine(i);
                }
            }
            for (; i < 10000000; i++)
            {
                navazovania.Add(new Models.Navazovanie
                {
                    ZariadenieId = 1,
                    DatumStartu = DateTime.Now.AddDays(2),
                    CasStartu = DateTime.Now.AddDays(2).ToString("HH:mm"),
                    NavazeneMnozstvo = i,
                    OdkialId = 3,
                    KamId = 2,
                    MaterialId = 2,
                });
                if (i % 100000 == 0)
                {
                    context.Navazovania.AddRange(navazovania);
                    context.SaveChanges();
                    navazovania.Clear();
                    Console.WriteLine(i);
                }
            }
        }
    }
}
