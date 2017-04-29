using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Agent.Service.Hubs
{
    /// <summary>
    /// A SignalR hub for sending simple string messages.
    /// </summary>
    public class GeneralMessageHub : Hub
    {
        #region Public Methods
        /// <summary>
        /// Sends a string message to all connected clients.
        /// </summary>
        /// <param name="message">The message to send to the client</param>
        public void Send(string message)
        {
            Clients.All.sendMessage(message);
        }

        public void GetAgentStatus()
        {
            Clients.All.sendMessage("Get agent status response from SignalR for "  + Environment.MachineName);
        }
        #endregion
    }
}
