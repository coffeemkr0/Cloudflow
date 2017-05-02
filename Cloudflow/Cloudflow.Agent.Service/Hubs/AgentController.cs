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

        private static Agent _agent;

        #region Public Methods
        public AgentStatus GetAgentStatus()
        {
            try
            {
                if(_agent == null)
                {
                    return new AgentStatus
                    {
                        Status = AgentStatus.AgentStatuses.NotRunning,
                        StatusDisplayText = "Not Running"
                    };
                }

                //TODO:Get a real status of what the agent is doing here
                return new AgentStatus
                {
                    Status = AgentStatus.AgentStatuses.Idle,
                    StatusDisplayText = "Idle"
                };
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
                if(_agent == null)
                {
                    _agent = new Agent();
                }
                
                _agent.Start();
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
                _agent.Stop();
                _agent = null;
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex);
            }
        }
        #endregion
    }
}
