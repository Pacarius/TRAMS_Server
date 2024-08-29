using Config;
using CSharp.src.Generics;
using MySqlConnector;
using Server;

namespace DB{
    internal class DBConnector{
        DBConfig? dbConfig;
        readonly string connectionString;
        ILoggingHelper helper = new() { LoggingDepth = 2 };
        public DBConnector(){
            dbConfig = Program.configHelper.Configs.dbConfig.DBConfig;
            MySqlConnectionStringBuilder builder = new()
            {
                Server = dbConfig.Host,
                UserID = dbConfig.User,
                Password = dbConfig.Password,
                Database = dbConfig.Database,
                Port = dbConfig.Port,
                AllowPublicKeyRetrieval = true
            };
            connectionString = builder.ConnectionString;
        }
        internal async Task<MySqlConnection> GetConnection(){
            MySqlConnection conn = new(connectionString);
            await conn.OpenAsync();
            return conn;
        }
        internal async Task<int> ExeNonQ(MySqlCommand cmd)
        {
            using MySqlConnection conn = new(connectionString);
            await conn.OpenAsync();
            cmd.Connection = conn;
            helper.Log($"Executing {cmd.CommandText}", this);
            var r =  await cmd.ExecuteNonQueryAsync();
            // await conn.DisposeAsync();
            return r;
        }
        internal async Task OverrideData(){
            using MySqlConnection conn = new(connectionString);
            await conn.OpenAsync();
            MySqlBatch batch = DBCommands.DataBatch(conn, Program.configHelper.Configs.VehiclesConfig, Program.configHelper.Configs.LamppostsConfig, dbConfig);
            await batch.ExecuteNonQueryAsync();
        }
        internal async Task InsertLogs(EspHolder holder){
            using MySqlConnection conn = new(connectionString);
            await conn.OpenAsync();
            MySqlBatch batch = DBCommands.LogBatch(conn, holder);
            await batch.ExecuteNonQueryAsync();
        }
    }
}