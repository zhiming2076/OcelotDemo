using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Hubs
{
    /// <summary>
    /// 诊断集线器
    /// </summary>
    public class WapDiagnosticHub: Hub
    {
        public Task SendDiagnostics(string info)
        {
            return Clients.All.InvokeAsync("OnSendDiagnostics", info);
        }
    }
}
