using Config;
using CSharp.src.Generics;
using DB;

internal class Program
{
    public static ConfigHelper? configHelper;
    public static DBConnector? dBConnector;
    static ILoggingHelper helper = new() { LoggingDepth = 1 };
    static async Task Main(string[] args)
    {
        await MainLoop();
    }
    static ConsoleKey Lock()
    {
        while (true)
        {
            ConsoleKey key = Console.ReadKey().Key;
            if (AcceptedKeys.Contains(key)) return key;
        }
    }
    static readonly ConsoleKey[] AcceptedKeys = [ConsoleKey.Q, ConsoleKey.R];
    async static Task MainLoop()
    {
        while (true)
        {
            if (configHelper == null) configHelper = await ConfigHelper.CreateHelper();
            else await configHelper.ReadFile();
            dBConnector = new DBConnector();
            await dBConnector.OverrideData();
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
}
