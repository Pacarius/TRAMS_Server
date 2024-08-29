using System.ComponentModel.Design.Serialization;
using System.Text.Json;
using CSharp.src.Generics;
using Microsoft.AspNetCore.Http;
using MySqlConnector;

namespace DB{
    internal partial class DBCommands{
        static ILoggingHelper helper = new() { LoggingDepth = 2 };
        static DBConnector Connector = Program.dBConnector;
        static readonly string GetEventsString = "";
        static readonly string GetRString_Owner = "SELECT * FROM `CheckReservations` WHERE `OwnerName` = @OwnerName";
        static readonly string GetRString_Street = "SELECT `Beg_Datetime`, `Duration`, `X`, `Y`, `Status` FROM `CheckReservations` WHERE `street_NAME` = @Street";
        static readonly string GetVInfoString = "SELECT `ID`, `Type`, `OwnerName` FROM `vehicle` WHERE `ID` = @PlateNo";
        static readonly string GetViolationsString = "";
        static readonly string InsertReservationString = "INSERT INTO `reservation`(`Beg_Datetime`,`Duration`,`X`,`Y`,`street_NAME`,`vehicle_ID`) VALUES(@Datetime, @Duration, X, Y, @Street, @VehicleID)";
        static readonly string VRecentRecordsString = "";
        //Helper method for formatting queries to return raw JSONS.
        static async Task<string> QToJson(string Query, Dictionary<string, string> Params){
            MySqlCommand cmd = new()
            {
                CommandText = Query,
                Connection = await Connector.GetConnection()
            };
            foreach(var param in Params)
                cmd.Parameters.Add(new() { ParameterName = param.Key, Value = param.Value });
            using var r = await cmd.ExecuteReaderAsync();
            var results = new List<Dictionary<string, object>>();
            while (await r.ReadAsync())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < r.FieldCount; i++)
                {
                    row[r.GetName(i)] = r.GetValue(i);
                }
                results.Add(row);
            }
            return JsonSerializer.Serialize(results);
        }
        public static RequestDelegate getRStringOwner = async context => {
            string Owner = context.Request.Query["Owner"];
            string res = await QToJson(GetRString_Owner, new() { { "@OwnerName", Owner } });
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(res);
            helper.Log($"Owner: {res}", "DBCommands");
        };
        public static RequestDelegate getRStringStreet = async context => {
            string Street = context.Request.Query["Street"];
            string res = await QToJson(GetRString_Street, new() { { "@Street", Street } });
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(res);
            helper.Log($"Street: {res}", "DBCommands");
        };
        public static RequestDelegate getVInfo = async context => {
            string PlateNo = context.Request.Query["PlateNo"];
            string res = await QToJson(GetVInfoString, new() { { "@PlateNo", PlateNo } });
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(res);
            helper.Log($"Vehicle: {res}", "DBCommands");
        };
        public static RequestDelegate insertReservation = async context => {
            Dictionary<string, string> Params = new(){
                {"Datetime", context.Request.Query["Datetime"]},
                {"Duration", context.Request.Query["Duration"]},
                {"X", context.Request.Query["X"]},
                {"Y", context.Request.Query["Y"]},
                {"Street", context.Request.Query["Street"]},
                {"VehicleID", context.Request.Query["VehicleID"]}
            };
            MySqlCommand cmd = new()
            {
                CommandText = InsertReservationString,
                Connection = await Connector.GetConnection()
            };
            foreach(var param in Params)
                cmd.Parameters.Add(new() { ParameterName = param.Key, Value = param.Value });
                try{await cmd.ExecuteNonQueryAsync();}catch(Exception e){helper.Log(e.Message, "DBCommands");context.Response.StatusCode = 500;}
            helper.Log($"Reservation: {Params["Datetime"]}, {Params["Duration"]}, {Params["X"]}, {Params["Y"]}, {Params["Street"]}, {Params["VehicleID"]}", "DBCommands");
        };
    }
}