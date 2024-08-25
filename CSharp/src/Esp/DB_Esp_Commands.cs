using MySqlConnector;
using Server;

namespace DB
{
    internal partial class DBCommands
    {
        static readonly string VehicleLogInsertString = "CALL vehiclelog_insert(@PlateNo, @X, @Y, @Anchors)";
        public static MySqlBatch LogBatch(MySqlConnection conn, EspHolder holder)
        {
            MySqlBatch batch = new(conn);
            var sum = new VehicleSum(holder).GetSums();
            foreach (var vehicle in sum)
                batch.BatchCommands.Add(new MySqlBatchCommand(VehicleLogInsertString)
                {
                    Parameters = {
                        new() { ParameterName = "@PlateNo", Value = vehicle.Vehicle },
                        new() { ParameterName = "@X", Value = vehicle.X },
                        new() { ParameterName = "@Y", Value = vehicle.Y },
                        new() { ParameterName = "@Anchors", Value = string.Join(",", vehicle.Anchors) }
                    }
                });
            return batch;
        }
    }
}