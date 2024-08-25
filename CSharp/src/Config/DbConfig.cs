using System.Text.Json.Serialization;

namespace Config
{
    public class DBConfig(string host = "db", string user = "root", string password = "vtccenter", string database = "mydb", ushort port = 6969, bool echo = false)
    {
        [JsonRequired]
        public string Host {get; set;} = host;
        [JsonRequired]
        public string User {get; set;} = user;
        [JsonRequired]
        public string Password {get; set;} = password;
        [JsonRequired]
        public string Database {get; set;} = database;
        [JsonRequired]
        public ushort Port {get; set;} = port;
        [JsonRequired]
        public bool Echo {get; set;} = echo;
    }
    public class dbConfig{
        public DBConfig DBConfig {get; set;} = new DBConfig();
    }
}