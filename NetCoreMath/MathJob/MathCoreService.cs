using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Sockets.Client;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR.Internal.Protocol;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
namespace NetCoreMath.MathJob
{
    public interface IMathCoreService
    {
        void Excute();
        Task Start();
    }

    public class MathCoreService : IMathCoreService
    {

        public static BlockingCollection<int> blockingCollection = new BlockingCollection<int>();
        IConnection connection; IHubProtocol protocol; ILoggerFactory loggerFactory;
        AppConfig appConfig;
        HubConnection hub;
        string hostName;
        int taskId = 0;
        public MathCoreService(IOptions<AppConfig> setting)
        {
            appConfig = setting.Value;
            this.hostName = appConfig.HostName;
            connection = new HttpConnection(new Uri(appConfig.SignalrConfig.Url));
            protocol = new JsonHubProtocol();
            loggerFactory = new LoggerFactory();
            hub = new HubConnection(connection, protocol, loggerFactory);


        }
        public bool isStart = false;


        public async Task Start()
        {
            await hub.StartAsync();

        }
        public async void Excute()
        {

            if (blockingCollection.Count > 0)
            {
                try
                {
                    if (!isStart)
                    {
                        await Start();
                        isStart = true;
                    }
                    await hub.InvokeAsync(appConfig.SignalrConfig.MethodName, DateTime.Now.ToString(), $"{hostName}正在执行计算服务!任务数量：{blockingCollection.Count}", taskId);
                    while (blockingCollection.Count > 0)
                    {
                        int temp = 0;
                        blockingCollection.TryTake(out temp);
                        await Task.Delay(10);

                    }
                    await hub.InvokeAsync(appConfig.SignalrConfig.MethodName, DateTime.Now.ToString(), $"{hostName}完成计算服务!", taskId++);
                }
                catch (Exception)
                {


                }
            }

            Console.WriteLine($"{DateTime.Now} 正在执行定时job");
        }
    }
}
