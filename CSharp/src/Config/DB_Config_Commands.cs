using Config;
using MySqlConnector;
using Server;

namespace DB{
    internal partial class DBCommands{
        static readonly string StreetInsertString = "INSERT INTO street (NAME) VALUES (@Name)";
        static readonly string VehicleInsertString = "INSERT INTO vehicle (ID, OwnerName, OwnerID, Type) VALUES (@PlateNo, @OwnerName, @OwnerID, @Type)";
        static readonly string LamppostInsertString = "INSERT INTO lamppost (ID, X, Y, street_NAME) VALUES (@ID, @X, @Y, @Street)";
        static readonly string ResUpdateString = "call setSens(@Sens); call setLeeway(@Leeway);";
        public static MySqlBatch DataBatch(MySqlConnection connection, VehiclesConfig vehicles, LamppostsConfig lampposts, DBConfig dbConfig){
            MySqlBatch batch = new(connection);
            batch.BatchCommands.Add(new MySqlBatchCommand("SET foreign_key_checks = 0;"));
            batch.BatchCommands.Add(new MySqlBatchCommand("DELETE FROM street"));
            foreach(var street in lampposts.Streets)
                batch.BatchCommands.Add(new MySqlBatchCommand(StreetInsertString){
                    Parameters = { new() { ParameterName = "@Name", Value = street } }
                });
            // Console.WriteLine(lampposts.Streets[0]);
            batch.BatchCommands.Add(new MySqlBatchCommand("DELETE FROM lamppost"));
            foreach(var lamppost in lampposts.Lampposts)
                batch.BatchCommands.Add(new MySqlBatchCommand(LamppostInsertString){
                    Parameters = {
                        new() { ParameterName = "@ID", Value = lamppost.ID },
                        new() { ParameterName = "@X", Value = lamppost.X },
                        new() { ParameterName = "@Y", Value = lamppost.Y },
                        new() { ParameterName = "@Street", Value = lamppost.Street }
                    }
                });
            // Console.WriteLine(lampposts.Lampposts[0].Street);
            batch.BatchCommands.Add(new MySqlBatchCommand("DELETE FROM vehicle"));
            foreach(var vehicle in vehicles.Vehicles)
                batch.BatchCommands.Add(new MySqlBatchCommand(VehicleInsertString){
                    Parameters = {
                        new() { ParameterName = "@PlateNo", Value = vehicle.PlateNo },
                        new() { ParameterName = "@OwnerName", Value = vehicle.OwnerName },
                        new() { ParameterName = "@OwnerID", Value = vehicle.OwnerID },
                        new() { ParameterName = "@Type", Value = vehicle.ReturnType() }
                    }
                });
            batch.BatchCommands.Add(new MySqlBatchCommand("SET foreign_key_checks = 1;"));
            batch.BatchCommands.Add(new MySqlBatchCommand(ResUpdateString){
                Parameters = {
                    new() { ParameterName = "@Sens", Value = dbConfig.ResSensitivity },
                    new() { ParameterName = "@Leeway", Value = dbConfig.Leeway }
                }
            });
            return batch;
        }
    }}