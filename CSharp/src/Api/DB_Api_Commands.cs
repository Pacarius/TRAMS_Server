
using System.Text.Json;
using CSharp.src.Generics;
using Microsoft.AspNetCore.Http;
using MySqlConnector;

namespace DB
{
    internal partial class DBCommands
    {
        static ILoggingHelper helper = new() { LoggingDepth = 2 };
        static DBConnector Connector = Program.dBConnector;
        static readonly string GetEventsString = "";
        static readonly string GetRString_Owner = "SELECT * FROM `CheckReservations` WHERE `OwnerName` = @Name";
        static readonly string GetRString_Street = "SELECT `Beg_Datetime`, `Duration`, `X`, `Y`, `Status`, `Type` FROM `CheckReservations` WHERE `street_NAME` = @Street";
        static readonly string GetVQRString = "SELECT `ID`, `Type`, `OwnerName` FROM `vehicle` WHERE `ID` = @PlateNo";
        static readonly string GetVOString = "SELECT * FROM `vehicle` WHERE `OwnerName` = @Name";
        static readonly string GetViolationsString = "";
        static readonly string InsertReservationString = "INSERT INTO `reservation`(`Beg_Datetime`,`Duration`,`X`,`Y`,`street_NAME`,`vehicle_ID`) VALUES(@Datetime, @Dur, @_X, @_Y, @Street, @VehicleID)";
        static readonly string VRecentRecordsString = "";
        //Helper method for formatting queries to return raw JSONS.
        static async Task<string> QToJson(string Query, Dictionary<string, string> Params)
        {
            try
            {
                MySqlCommand cmd = new(Query, await Connector.GetConnection());
                foreach(var pair in Params) {cmd.Parameters.AddWithValue(pair.Key, pair.Value); helper.Log($"{pair.Key} : {pair.Value}", "DBCommands");}
                helper.Log($"{Query}; {cmd.Parameters.Count}", "DBCommands"); 
                using var r = await cmd.ExecuteReaderAsync();
                var results = new List<Dictionary<string, object>>();
                while (await r.ReadAsync())
                {
                    var row = Enumerable.Range(0, r.FieldCount)
                        .ToDictionary(r.GetName, r.GetValue);
                    results.Add(row);
                }
                return JsonSerializer.Serialize(results);
            }
            catch (MySqlException e) { helper.Log(e.Message, "DBCommands"); return e.Message; }
        }
        static async Task RequestHandler(string Query, List<(string HttpName, string ParameterName)> values, HttpContext context)
        {
            var Params = values.ToDictionary(c => c.ParameterName, c => context.Request.Query[c.HttpName].ToString());
            var Res = await QToJson(Query, Params);
            context.Response.ContentType = "application/json";
            helper.Log($"{Res}", "DBCommands");
            await context.Response.WriteAsync(Res);
        }
        [Api.Routes("getro")]
        public static async Task GetRSOwner(HttpContext context) => await RequestHandler(GetRString_Owner, [("owner", "@Name")], context);
        [Api.Routes("getrstreet")]
        public static async Task GetRStreet(HttpContext context) => await RequestHandler(GetRString_Street, [("street", "@Street")], context);
        [Api.Routes("getvo")]
        public static async Task GetVOwner(HttpContext context) => await RequestHandler(GetVOString, [("owner", "@Name")], context);
        [Api.Routes("getvqr")]
        public static async Task GetVInfoQR(HttpContext context) => await RequestHandler(GetVQRString, [("plateNo", "@PlateNo")], context);
        [Api.Routes("insertreservation")]
        public static async Task InsertReservation(HttpContext context) => await RequestHandler(InsertReservationString, [("datetime", "@Datetime"), ("Duration", "@Dur"), ("X", "@_X"), ("Y", "@_Y"), ("Street", "@Street"), ("VehicleID", "@VehicleID")], context);
    }
}