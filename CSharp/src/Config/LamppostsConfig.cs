using System.Text.Json.Serialization;

namespace Config{
    public class LamppostsConfig(List<Lamppost>? lampposts)
    {
        [JsonRequired]
        public List<Lamppost> Lampposts { get; set; } = lampposts ?? ([
            new Lamppost(){ID = "000000", X = 0, Y = 0, Street = "TEST"},
            new Lamppost(){ID = "000001", X = 0, Y = 0, Street = "TEST"}
            ]);
    }
    public class Lamppost(string iD = "NULL", double x = 0.0, double y = 0.0, string street = "NULL")
    {
        [JsonRequired]
        public string ID { get; set; } = iD;
        [JsonRequired]
        public double X { get; set; } = x;
        [JsonRequired]
        public double Y { get; set; } = y;
        [JsonRequired]
        public string Street { get; set; } = street;
    }
}