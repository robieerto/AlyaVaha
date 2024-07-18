using AlyaVaha;
using Photino.NET;
using PhotinoNET.Server;
using System.Drawing;
using System.Text;

namespace Photino.HelloPhotino.Vue;

class Program
{
#if DEBUG
    public static bool IsDebugMode = true;
#else
    public static bool IsDebugMode = false;
#endif

    [STAThread]
    static void Main(string[] args)
    {
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
            // Center window in the middle of the screen
            .Center()
            // Users can resize windows by default.
            // Let's make this one fixed instead.
            .SetResizable(true)
            .SetContextMenuEnabled(IsDebugMode)
            .SetDevToolsEnabled(IsDebugMode)
            .RegisterCustomSchemeHandler("app", (object sender, string scheme, string url, out string contentType) =>
            {
                contentType = "text/javascript";
                return new MemoryStream(Encoding.UTF8.GetBytes(@"
                        (() =>{
                            window.setTimeout(() => {
                                alert(`🎉 Dynamically inserted JavaScript.`);
                            }, 1000);
                        })();
                    "));
            })
            // Most event handlers can be registered after the
            // PhotinoWindow was instantiated by calling a registration 
            // method like the following RegisterWebMessageReceivedHandler.
            // This could be added in the PhotinoWindowOptions if preferred.
            .RegisterWebMessageReceivedHandler(MainMessageHandler.MessageHandler)
            .Load(appUrl); // Can be used with relative path strings or "new URI()" instance to load a website.

        window.LogVerbosity = 0;

        // Initialize the DataCommunicator
        DataCommunicator.Init(window);
        // Start the DataCommunicator
        Task.Run(async () => DataCommunicator.Run());

        window.WaitForClose(); // Starts the application event loop
    }
}
