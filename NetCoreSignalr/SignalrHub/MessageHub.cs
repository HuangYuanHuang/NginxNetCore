using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
namespace NetCoreSignalr.SignalrHub
{
    public class MessageHub : Hub
    {
        public void SendMessage(string time,string message,int task)
        {
            Clients.All.InvokeAsync("pushMessage", time, message,task);
        }
    }
}
