
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
        static readonly string GetRString_Owner = "SELECT * FROM `CheckReservations` WHERE `OwnerName` = OwnerName";
        static readonly string GetRString_Street = "SELECT `Beg_Datetime`, `Duration`, `X`, `Y`, `Status` FROM `CheckReservations` WHERE `street_NAME` = Street";
        static readonly string GetVInfoString = "SELECT `ID`, `Type`, `OwnerName` FROM `vehicle` WHERE `ID` = PlateNo";
        static readonly string GetViolationsString = "";
        static readonly string InsertReservationString = "INSERT INTO `reservation`(`Beg_Datetime`,`Duration`,`X`,`Y`,`street_NAME`,`vehicle_ID`) VALUES(Datetime, Duration, X, Y, Street, VehicleID)";
        static readonly string VRecentRecordsString = "";
        //Helper method for formatting queries to return raw JSONS.
        static async Task<string> QToJson(string Query, Dictionary<string, string> Params)
        {
            try
            {
                MySqlCommand cmd = new(Query, await Connector.GetConnection());
                _ = Params.Select(c => cmd.Parameters.AddWithValue(c.Key, c.Value));
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
            var Params = values.ToDictionary(c => c.HttpName, c => context.Request.Query[c.HttpName].ToString());
            var Res = await QToJson(Query, Params);
            context.Response.ContentType = "application/json";
            helper.Log($"Owner: {Res}", "DBCommands");
            await context.Response.WriteAsync(Res);
        }
        [Api.Routes("getrowner")]
        public static async Task GetRSOwner(HttpContext context) => await RequestHandler(GetRString_Owner, [("owner", "@OwnerName")], context);
        [Api.Routes("getrstreet")]
        public static async Task GetRStreet(HttpContext context) => await RequestHandler(GetRString_Street, [("street", "@Street")], context);
        [Api.Routes("getvinfo")]
        public static async Task GetVInfo(HttpContext context) => await RequestHandler(GetVInfoString, [("plateNo", "@PlateNo")], context);
        [Api.Routes("insertreservation")]
        public static async Task InsertReservation(HttpContext context) => await RequestHandler(InsertReservationString, [("datetime", "@Datetime"), ("Duration", "@Duration"), ("X", "@X"), ("Y", "@Y"), ("Street", "@Street"), ("VehicleID", "@VehicleID")], context);
    }
}