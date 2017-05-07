using Cloudflow.Core;
using Cloudflow.Core.Data.Agent;
using Cloudflow.Core.Data.Agent.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Runtime.Hubs
{
    public class AgentController : Hub
    {
        private static readonly log4net.ILog _logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static Agent _agent;
        private static object _agentSynch = new object();

        #region Private Methods
        private void _agent_StatusChanged(AgentStatus status)
        {
            Clients.All.updateStatus(status);
        }
        #endregion

        #region Public Methods
        public AgentStatus GetAgentStatus()
        {
            try
            {
                if(_agent == null)
                {
                    return new AgentStatus
                    {
                        Status = AgentStatus.AgentStatuses.NotRunning
                    };
                }

                return _agent.AgentStatus;
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex);
            }

            return null;
        }

        public void StartAgent()
        {
            try
            {
                lock (_agentSynch)
                {
                    if (_agent == null)
                    {
                        _logger.Info("Creating a test agent");
                        _agent = Agent.CreateTestAgent();
                        _agent.StatusChanged += _agent_StatusChanged;
                        _agent.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex);
            }
        }

        public void StopAgent()
        {
            try
            {
                lock (_agentSynch)
                {
                    if (_agent != null)
                    {
                        _agent.Stop();
                        _agent = null;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex);
            }
        }

        public List<Run> GetCompletedRuns()
        {
            using (AgentDbContext agentDbContext = new AgentDbContext())
            {
                return agentDbContext.Runs.Where(i => i.Status == Run.RunStatuses.Completed ||
                    i.Status == Run.RunStatuses.Completed || i.Status == Run.RunStatuses.Failed ||
                    i.Status == Run.RunStatuses.Canceled).ToList();
            }
        }

        public List<Run> GetQueuedRuns()
        {
            using (AgentDbContext agentDbContext = new AgentDbContext())
            {
                return agentDbContext.Runs.Where(i => i.Status == Run.RunStatuses.Queued ||
                    i.Status == Run.RunStatuses.Running).ToList();
            }
        }
        #endregion
    }
}
