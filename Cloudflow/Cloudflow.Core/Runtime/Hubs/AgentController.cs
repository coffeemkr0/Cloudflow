using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using Cloudflow.Core.Data.Agent;
using Cloudflow.Core.Data.Agent.Models;
using Cloudflow.Core.Data.Shared.Models;
using log4net;
using System.Reflection;
using Cloudflow.Core.Extensions.Controllers;

namespace Cloudflow.Core.Runtime.Hubs
{
    public class AgentController : Hub
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static Agent _agent;
        private static object _agentControlSynch = new object();
        private static object _publishJobSynch = new object();

        #region Private Methods
        private List<JobController> GetJobControllers()
        {
            var jobControllers = new List<JobController>();

            using (var agentDbContext = new AgentDbContext())
            {
                foreach (var jobDefinition in agentDbContext.JobDefinitions)
                {
                    Logger.Info($"Add job {jobDefinition.JobDefinitionId}");

                    var jobController = new JobController(jobDefinition);
                    Logger.Info($"Loading job {jobController.JobConfiguration.Name}");

                    jobControllers.Add(jobController);
                }
            }

            return jobControllers;
        }

        private void _agent_StatusChanged(AgentStatus status)
        {
            Clients.All.updateStatus(status);
        }

        private void _agent_RunStatusChanged(Run run)
        {
            Clients.All.runStatusChanged(run);
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
                Logger.Fatal(ex);
            }

            return null;
        }

        public void PublishJob(JobDefinition jobDefinition)
        {
            lock (_publishJobSynch)
            {
                using (var agentDbContext = new AgentDbContext())
                {
                    var existingJobDefinition = agentDbContext.JobDefinitions.FirstOrDefault(i => i.JobDefinitionId == jobDefinition.JobDefinitionId);
                    if (existingJobDefinition == null)
                    {
                        agentDbContext.JobDefinitions.Add(jobDefinition);
                        agentDbContext.SaveChanges();
                    }
                    else
                    {
                        agentDbContext.JobDefinitions.Remove(existingJobDefinition);
                        agentDbContext.JobDefinitions.Add(jobDefinition);
                        agentDbContext.SaveChanges();
                    }
                }

                if(_agent != null)
                {
                    StopAgent();
                    StartAgent();
                }
            }
        }

        public void StartAgent()
        {
            try
            {
                lock (_agentControlSynch)
                {
                    if (_agent == null)
                    {
                        Logger.Info("Starting agent");

                        _agent = new Agent(GetJobControllers());
                        _agent.StatusChanged += _agent_StatusChanged;
                        _agent.RunStatusChanged += _agent_RunStatusChanged;
                        _agent.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
            }
        }

        public void StopAgent()
        {
            try
            {
                lock (_agentControlSynch)
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
                Logger.Fatal(ex);
            }
        }

        public List<Run> GetCompletedRuns(int startIndex, int pageSize)
        {
            using (var agentDbContext = new AgentDbContext())
            {
                return agentDbContext.Runs.Where(i => i.Status == Run.RunStatuses.Completed ||
                    i.Status == Run.RunStatuses.Completed || i.Status == Run.RunStatuses.Failed ||
                    i.Status == Run.RunStatuses.Canceled).OrderByDescending(i => i.DateEnded)
                    .Skip(startIndex).Take(pageSize).ToList();
            }
        }

        public List<Run> GetQueuedRuns()
        {
            if(_agent == null)
            {
                return new List<Run>();
            }
            else
            {
                return _agent.GetQueuedRuns();
            }
        }
        #endregion
    }
}
