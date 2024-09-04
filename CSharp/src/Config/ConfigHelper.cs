using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using CSharp.src.Generics;
using Microsoft.AspNetCore.Mvc.Razor;
namespace Config
{
    public class ConfigHelper
    {
        static ILoggingHelper helper = new() { LoggingDepth = 1 };
        readonly JsonSerializerOptions jsonOptions = new() { WriteIndented = true,
        //    TypeInfoResolver = new DefaultJsonTypeInfoResolver
        //    {
        //        Modifiers = {
        //    static typeInfo => {
        //    //if (typeInfo.Kind != JsonTypeInfoKind.Object)
        //    //        return;

        //        foreach (JsonPropertyInfo propertyInfo in typeInfo.Properties)
        //        {
        //            propertyInfo.IsRequired = true;
        //        }
        //    }
        //}
        //    }
        };
        static string ConfigPath = "config/Config.json";
        public int LoggingDepth => Configs.tramsConfig.TRAMSConfig.LoggingLevel;
        public ConfigHolder Configs { get; private set; } = new(){
            VehiclesConfig = new VehiclesConfig(null),
            dbConfig = new dbConfig(),
            tramsConfig = new tramsConfig()
        };
        public static async Task<ConfigHelper> CreateHelper()
        {
            var tmp = new ConfigHelper();
                if(!Directory.Exists("config")) Directory.CreateDirectory("config");
                helper.Log(Directory.GetCurrentDirectory(), "ConfigHelper");
                if (!File.Exists(ConfigPath))
                { 
                    await tmp.WriteFile(); 
                } else await tmp.ReadFile();
                return tmp;
        }
        public async Task ReadFile()
        {
            helper.LogDirect("Loading Configuration");
            await using FileStream cS = File.OpenRead(ConfigPath);
            try
            {
                var tmp = await JsonSerializer.DeserializeAsync<ConfigHolder>(cS);
                if (tmp != null) Configs = tmp;
            }
            catch (JsonException)
            {
                cS.Close();
                await WriteFile();
                Console.WriteLine("Detected errors/missing attributes in config file. Regenerating with defaults.");
            }
        }
        public async Task WriteFile()
        {
            await using FileStream cS = File.OpenWrite(ConfigPath);
            await JsonSerializer.SerializeAsync(cS, Configs, options: jsonOptions);
        }
    }
    public class ConfigHolder
    {
        [JsonRequired]
        public VehiclesConfig VehiclesConfig { get; set; } = new VehiclesConfig(null);
        [JsonRequired]
        public dbConfig dbConfig { get; set; } = new dbConfig();
        [JsonRequired]
        public tramsConfig tramsConfig { get; set; } = new tramsConfig();
        [JsonRequired]
        public LamppostsConfig LamppostsConfig { get; set; } = new LamppostsConfig(null, null);
    }
}