using AlyaLibrary;
using AlyaVaha;
using AlyaVaha.DAL;
using DeviceId;
using Microsoft.Extensions.Configuration;
using Photino.NET;
using PhotinoNET.Server;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;

namespace Photino.HelloPhotino.Vue;

class Program
{
    private static readonly string initialKey = "5e8eeebe52f9a8226663bbbe8fee08aa";
    private static readonly string initialConfigFile = ".initConfig";

    public static string ConnectionString = "";

#if DEBUG
    public static bool IsDebugMode = true;
#else
    public static bool IsDebugMode = false;
#endif

    [STAThread]
    static void Main(string[] args)
    {
        try
        {
            // Read configuration file
            var config = new ConfigurationBuilder()
                .AddJsonFile("configuration.json", optional: false, reloadOnChange: false)
                .Build();

            ConnectionString = config.GetValue<string>("ConnectionString") ?? "";

            var zoom = config.GetValue<int>("Zoom");
            if (zoom == 0) zoom = 100;

            var timeout = config.GetValue<int>("Timeout");
            if (timeout == 0) timeout = 100;

            var lightTimeout = config.GetValue<int>("LightTimeout");
            if (lightTimeout == 0) lightTimeout = 1000;


            //var connectionString = config.GetConnectionString("DefaultConnection");

            // Check if initial config file exists
            if (File.Exists(initialConfigFile) && new FileInfo(initialConfigFile).Length != 0)
            {
                // Read the text file
                var initialConfigLines = File.ReadAllLines(initialConfigFile);
                var initialConfigKey = initialConfigLines[0];
                var zariadeniaCount = 1;
                try
                {
                    zariadeniaCount = int.Parse(initialConfigLines[^1]);
                }
                catch (Exception)
                {
                }

                if (initialConfigKey != null && initialConfigKey.Equals(initialKey))
                {
                    // Ensure the folder from connection string is created
                    var folder = Path.GetDirectoryName(ConnectionString);
                    // Delete everything before '='
                    folder = folder?.Substring(folder.IndexOf('=') + 1);
                    if (folder?.Length > 0 && !Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    using var context = new AlyaVahaDbContext();
                    // Create the database
                    context.Database.EnsureCreated();

                    using var sha256 = SHA256.Create();

                    // Generate PC ID and hash it
                    var pcId = GetPcId();
                    var pcIdString = $"{initialKey} {pcId}";
                    var pcIdHash = GetHash(sha256, pcIdString);

                    // Generate hash of the devices
                    var zariadeniaString = $"{pcIdString} {zariadeniaCount}";
                    var zariadeniaHash = GetHash(sha256, zariadeniaString);

                    context.Programy.Update(new AlyaVaha.Models.Program { Id = 1, PcId = pcIdHash, Zariadenia = zariadeniaHash });

                    var zariadeniaCountDb = context.Zariadenia.Count();
                    if (zariadeniaCount > zariadeniaCountDb)
                    {
                        for (int i = zariadeniaCountDb + 1; i <= zariadeniaCount; i++)
                        {
                            context.Zariadenia.Add(new AlyaVaha.Models.Zariadenie
                            {
                                NazovZariadenia = $"Váha {i}",
                                IpAdresa = "192.168.1.10",
                                Port = 3396,
                                PocetNavazeni = 0,
                                NavazeneMnozstvo = 0,
                                NavazenyPocetDavok = 0
                            });
                        }
                    }
                    context.SaveChanges();

                    // Remove content of file
                    File.WriteAllText(initialConfigFile, "");
                }
            }

            //Console.WriteLine("Applying seed...");
            //DbSeed.Seed();
            //Console.WriteLine("Seed applied");

            using (var context = new AlyaVahaDbContext())
            {
                var program = context.Programy.FirstOrDefault();
                if (program != null)
                {
                    using var sha256 = SHA256.Create();

                    // Check if the PC ID is the same
                    var pcId = GetPcId();
                    var pcIdString = $"{initialKey} {pcId}";
                    if (!VerifyHash(sha256, pcIdString, program.PcId!))
                    {
                        Library.WriteLog("Nesprávne PC ID");
                        return;
                    }

                    // Check if the hash of the devices is the same
                    var zariadeniaCount = context.Zariadenia.Count();
                    var zariadeniaString = $"{pcIdString} {zariadeniaCount}";

                    if (!VerifyHash(sha256, zariadeniaString, program.Zariadenia!))
                    {
                        Library.WriteLog("Nesprávny počet zariadení");
                        return;
                    }
                }
            }

            PhotinoServer
                .CreateStaticFileServer(args, out string baseUrl)
                .RunAsync();

            // The appUrl is set to the local development server when in debug mode.
            // This helps with hot reloading and debugging.
            string appUrl = IsDebugMode ? "http://localhost:5173" : $"{baseUrl}/index.html";
            Console.WriteLine($"Serving Vue app at {appUrl}");

            // Window title declared here for visibility
            string windowTitle = "Alya Váha";

            // Creating a new PhotinoWindow instance with the fluent API
            var window = new PhotinoWindow()
                .SetTitle(windowTitle)
                // Resize to a percentage of the main monitor work area
                //.Resize(50, 50, "%")
                .SetUseOsDefaultSize(false)
                .SetMaximized(true)
                .SetSize(new Size(1000, 800))
                .SetZoom(zoom)
                // Center window in the middle of the screen
                .Center()
                // Users can resize windows by default.
                // Let's make this one fixed instead.
                .SetResizable(true)
                .SetContextMenuEnabled(IsDebugMode)
                .SetDevToolsEnabled(IsDebugMode)
                //.RegisterCustomSchemeHandler("app", (object sender, string scheme, string url, out string contentType) =>
                //{
                //    contentType = "text/javascript";
                //    return new MemoryStream(Encoding.UTF8.GetBytes(@"
                //            (() =>{
                //                window.setTimeout(() => {
                //                    alert(`🎉 Dynamically inserted JavaScript.`);
                //                }, 1000);
                //            })();
                //        "));
                //})
                // Most event handlers can be registered after the
                // PhotinoWindow was instantiated by calling a registration 
                // method like the following RegisterWebMessageReceivedHandler.
                // This could be added in the PhotinoWindowOptions if preferred.
                .RegisterWebMessageReceivedHandler(MainMessageHandler.MessageHandler)
                .Load(appUrl); // Can be used with relative path strings or "new URI()" instance to load a website.

            window.LogVerbosity = 0;

            // Initialize the DataCommunicator
            DataCommunicator.Init(window, timeout, lightTimeout);
            // Start the DataCommunicator
            Task.Run(async () => DataCommunicator.Run());

            window.WaitForClose(); // Starts the application event loop

        }
        catch (Exception ex)
        {
            Library.WriteLog(ex);
        }
    }

    private static string GetPcId()
    {
        string deviceId = new DeviceIdBuilder()
            //.AddMachineName()
            //.AddOsVersion()
            .OnWindows(windows => windows
                .AddProcessorId()
                .AddMotherboardSerialNumber()
                .AddSystemDriveSerialNumber())
            .OnLinux(linux => linux
                .AddMotherboardSerialNumber()
                .AddSystemDriveSerialNumber())
            .OnMac(mac => mac
                .AddSystemDriveSerialNumber()
                .AddPlatformSerialNumber())
            .ToString();

        return deviceId;
    }

    private static string GetHash(HashAlgorithm hashAlgorithm, string input)
    {

        // Convert the input string to a byte array and compute the hash.
        byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        var sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString();
    }

    // Compare two strings by comparing their byte values.
    private static bool CompareStrings(string a, string b)
    {
        return a.Equals(b, StringComparison.OrdinalIgnoreCase);
    }

    // Verify a hash against a string.
    private static bool VerifyHash(HashAlgorithm hashAlgorithm, string input, string hash)
    {
        // Hash the input.
        var hashOfInput = GetHash(hashAlgorithm, input);

        return CompareStrings(hashOfInput, hash);
    }
}
