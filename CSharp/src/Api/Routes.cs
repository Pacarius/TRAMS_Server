using System.ComponentModel.Design.Serialization;
using Microsoft.AspNetCore.Http;

namespace Api{
    public class Routes{
        //Routes don't need leading slash(/)
        public static readonly Dictionary<string, RequestDelegate> RouteList = new(){
            {"A", async context => {await context.Response.WriteAsync("A");}},
        };
    }
}