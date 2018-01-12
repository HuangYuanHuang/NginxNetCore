using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Sockets.Client;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR.Internal.Protocol;

namespace NetCoreMath.MathJob
{
    public interface IMathCoreService
    {
        void Excute();
    }

    public class MathCoreService : IMathCoreService
    {
        static string SignalrUrl = "";
        IHubConnectionBuilder nu;
        IConnection connection; IHubProtocol protocol; ILoggerFactory loggerFactory;

        HubConnection hub;
        public MathCoreService()
        {
            connection = new HttpConnection(new Uri(""));
            protocol = new JsonHubProtocol();
            loggerFactory = new LoggerFactory();
            hub = new HubConnection(connection, protocol, loggerFactory);
            
        }

        public async Task Start()
        {
            await hub.StartAsync();
        }
        public void Excute()
        {

            Console.WriteLine($"{DateTime.Now} 正在执行定时job");
        }
    }
}
