namespace AlyaLibrary
{
    public static class Library
    {
        public static void WriteLog(Exception ex)
        {
            try
            {
                using var sw = new StreamWriter("error.log", true);
                sw.WriteLine(
                    "=>{0} An Error occurred: {1}{3}Message: {2}{3}",
                    DateTime.Now,
                    ex.StackTrace,
                    ex.Message,
                    Environment.NewLine
                );
            }
            catch
            {
            }
        }

        public static void WriteLog(string message)
        {
            try
            {
                using var sw = new StreamWriter("error.log", true);
                sw.WriteLine(
                    "=>{0} Message: {1}{2}",
                    DateTime.Now,
                    message,
                    Environment.NewLine
                );
            }
            catch
            {
            }
        }
    }
}
