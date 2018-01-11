using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
namespace NetCoreMath.MathJob
{
    public interface IMathCoreService
    {
        void Excute();
    }

    public class MathCoreService : IMathCoreService
    {
        public void Excute()
        {
          //  Microsoft.AspNetCore.SignalR.Client.HubConnection hub = new HubConnection();
            Console.WriteLine($"{DateTime.Now} 正在执行定时job");
        }
    }
}
