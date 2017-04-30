using Cloudflow.Core;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Agent.Service.Hubs
{
    public class AgentController : Hub
    {
        private static readonly log4net.ILog _logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Job _job;

        #region Public Methods
        public void GetAgentStatus()
        {
            Clients.All.sendMessage("Get agent status response from SignalR for " + Environment.MachineName);
        }

        public void EnableJob()
        {
            if (_job == null)
            {
                _logger.Debug("Enabling the job");
                _job = Job.CreateTestJob();
                _job.Enable();
                _logger.Info("Job enabled");

                Clients.All.sendMessage("Job enabled on " + Environment.MachineName);
            }
        }
        #endregion
    }
}
