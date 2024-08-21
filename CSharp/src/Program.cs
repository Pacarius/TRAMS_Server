using Config;
using CSharp.src.Generics;

internal class Program
{
    public static ConfigHelper? configHelper;
    static ILoggingHelper helper = new() { LoggingDepth = 1 };
    static async Task Main(string[] args)
    {
        while (true)
        {
            if (configHelper == null) configHelper = await ConfigHelper.CreateHelper();
            else await configHelper.ReadFile();
            var server = new Server.Server();
            helper.LogDirect($"Server started using TCP port:{configHelper.Configs.tramsConfig.TRAMSConfig.EspPort} and HTTP port:{configHelper.Configs.tramsConfig.TRAMSConfig.ApiPort}.");
            helper.LogDirect("Press Q to exit or R to reload.");
                ConsoleKey tmp = Lock();
                switch (tmp)
                {
                    case ConsoleKey.Q:
                        await server.Host.StopAsync();
                        return;
                    case ConsoleKey.R:
                        await server.Stop();
                        Console.Clear();
                        break;
                }
            }
    }
    static ConsoleKey Lock(){
        while(true){
            ConsoleKey key = Console.ReadKey().Key;
            if(key == ConsoleKey.Q) return key;
            else if(key == ConsoleKey.R) return key;
        }
    }
}
