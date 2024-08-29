using System.ComponentModel.Design.Serialization;
using DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api{
    public class Routes{
        //Routes don't need leading slash(/)
        public static readonly Dictionary<string, RequestDelegate> RouteList = new(){
            {"A", async context => {await context.Response.WriteAsync("A");}},
            {"getrstring_owner", DBCommands.getRStringOwner},
            {"getrstring_street", DBCommands.getRStringStreet},
            {"getvinfo", DBCommands.getVInfo},
            {"insertreservation", DBCommands.insertReservation},
            };
    }
}