using System.Text.Json.Serialization;

namespace Config
{
    public class TRAMSConfig(int logThreshold = 3)
    {
        [JsonRequired]
        public int LogThreshold {get; set;} = logThreshold;
        [JsonRequired]
        public ushort EspPort {get; set;} = 42069;
        [JsonRequired]
        public ushort ApiPort {get; set;} = 9696;
        [JsonRequired]
        public int LoggingLevel { get; set;} = 1;
    }
    public class tramsConfig{
        public TRAMSConfig TRAMSConfig {get; set;}= new TRAMSConfig();
    }
}