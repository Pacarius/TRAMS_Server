using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Api
{
        public class Routes
    {
        // Routes don't need leading slash(/)
        public static readonly Dictionary<string, RequestDelegate> RouteList = [];

        static Routes()
        {
            var methods = typeof(DB.DBCommands).GetMethods()
                .Where(m => m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(HttpContext) && m.ReturnType == typeof(Task));

            RouteList = methods
                .Select(method => new { Method = method, Attribute = method.GetCustomAttribute<RoutesAttribute>() })
                .Where(x => x.Attribute != null)
                .ToDictionary(x => x.Attribute.RouteName, x => CreateDelegate(x.Method));
        }
        private static RequestDelegate CreateDelegate(MethodInfo method)=>
            async context => await (Task)method.Invoke(Activator.CreateInstance(method.DeclaringType), [context]);
        
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    sealed class RoutesAttribute(string routeName) : Attribute
    {
        public string RouteName { get; } = routeName;
    }
}