using AlyaVaha.Models;
using System.Globalization;

namespace AlyaVaha
{
    public static class NavazovanieParser
    {
        public static Navazovanie Parse(string input)
        {
            var parts = input.Split(';');
            if (parts.Length < 13) throw new ArgumentException("Input string does not contain enough parts.");

            var navazovanie = new Navazovanie
            {
                Id = int.Parse(parts[0]),
                DatumStartu = TryParseDateTime(parts[1]),
                DatumKonca = TryParseDateTime(parts[2]),
                NavazeneMnozstvo = TryParseDouble(parts[3]),
                NavazenyPocetDavok = TryParseInt(parts[4]),
                PozadovaneMnozstvo = TryParseDouble(parts[5]),
                PozadovanyPocetDavok = TryParseInt(parts[6]),
                VelkostDavky = TryParseDouble(parts[7]),
                OdkialId = TryParseInt(parts[8]),
                KamId = TryParseInt(parts[9]),
                MaterialId = TryParseInt(parts[10]),
                //UzivatelId = TryParseInt(parts[11]) // Assuming USR1 maps to UzivatelId based on the context
                // USR2 is not directly mapped in the model provided. Adjust as necessary.
            };

            navazovanie.CasStartu = navazovanie.DatumStartu?.ToString("HH:mm");
            navazovanie.CasKonca = navazovanie.DatumKonca?.ToString("HH:mm");

            return navazovanie;
        }

        private static DateTime? TryParseDateTime(string input)
        {
            // Define the exact format of the input date string
            var format = "dd.MM.yyyyHH:mm:ss";
            // Use CultureInfo.InvariantCulture as the date format is culture-agnostic
            if (DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
            {
                return result;
            }
            return null;
        }

        private static int? TryParseInt(string input)
        {
            if (int.TryParse(input, out var result))
            {
                return result;
            }
            return null;
        }

        private static double? TryParseDouble(string input)
        {
            if (double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
            {
                return result;
            }
            return null;
        }
    }

}

