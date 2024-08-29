using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.src.Generics
{
    internal class ILoggingHelper
    {
        virtual internal int LoggingDepth { get; set; }
        //public string GetStats(Dictionary<string, object> Stats);
        internal int NextDepth => LoggingDepth + 1;
        internal bool LoggingEnabled => Program.configHelper != null && (LoggingDepth <= Program.configHelper.LoggingDepth || Program.configHelper.LoggingDepth == -1);
        internal void Log(string message, object Source){
            if(LoggingEnabled) Console.WriteLine($"Depth:{LoggingDepth}; Source:{Source.GetType()}, Message:{message}");
        }
        internal void Log(string message, string Source){
            if(LoggingEnabled) Console.WriteLine($"Depth:{LoggingDepth}; Source:{Source}, Message:{message}");
        }
        internal void LogDirect(string message){if(LoggingEnabled) Console.WriteLine(message);}
    }
}
