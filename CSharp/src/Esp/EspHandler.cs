using System.Text;
using System.Text.Json;
using CSharp.src.Generics;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;
using Utilities;

namespace Server
{
    public class EspHandler(ILogger<EspHandler> logger) : ConnectionHandler
    {
        private readonly ILogger<EspHandler> _logger = logger;
        readonly ILoggingHelper helper = new() {LoggingDepth = 1};
        public override async Task OnConnectedAsync(ConnectionContext connection)
        {
            var Active = true;
            var stageController = new StageController();
            connection.ConnectionClosed.Register(() => Active = false);
            Response res = Response.Begin;
            if(helper.LoggingEnabled)_logger.LogInformation(connection.ConnectionId + " connected");
            while (Active)
            {
                var result = await connection.Transport.Input.ReadAsync();
                var buffer = result.Buffer;
                foreach (var segment in buffer)
                {
                    var payload = Encoding.UTF8.GetString(segment.Span);
                    helper.Log(Encoding.UTF8.GetString(segment.Span), this);
                    switch (stageController.Stage)
                    {
                        case 0:
                            if (ParseSecret(payload)) stageController.Next();
                            else { await connection.Transport.Output.WriteAsync(Encoding.UTF8.GetBytes(Response.Fail.ToString())); return; }
                            break;
                        case 1:
                            var holder = new EspHolder(payload, out res);
                            break;
                    //         break;
                    }

                }
                connection.Transport.Input.AdvanceTo(buffer.End);
                if(res != Response.Begin) Active = false;
            }
            await connection.Transport.Output.WriteAsync(Encoding.UTF8.GetBytes(res.ToString()));
        }
        bool ParseSecret(string secret) => secret.Contains("ChiFatEntry");
    }
    public enum Response
    {
        Begin,
        Success,
        Partial,
        Empty,
        Fail
    }
}
