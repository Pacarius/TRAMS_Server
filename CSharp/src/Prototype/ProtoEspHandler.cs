using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CSharp.src.Prototype
{
    internal class ProtoEspHandler
    {
        //static JsonSerializerOptions options = new() { WriteIndented = true , IncludeFields = true};
        public static async Task<ProtoEspHandler> RunProtoHandler()
        {
            await using FileStream cS = File.OpenRead("SampleMessage.json");
            var A = new ProtoEspHandler();
            var B = new EspHolder(JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, Dictionary<string, double>>>>(cS), out Response r);
            return A;
        }
        
    }
}
