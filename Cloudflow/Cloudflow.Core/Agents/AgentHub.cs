using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Cloudflow.Core.Data.Agent;
using Cloudflow.Core.Data.Agent.Models;
using Cloudflow.Core.Data.Server;
using Cloudflow.Core.Data.Shared.Models;
using Cloudflow.Core.ExtensionManagement;
using Cloudflow.Core.Extensions;
using Cloudflow.Core.Jobs;
using Cloudflow.Core.Serialization;
using log4net;
using Microsoft.AspNet.SignalR;

namespace Cloudflow.Core.Agents
{
    public class AgentHub : Hub, IAgentMonitor
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static Agent _agent;
        private static readonly object AgentControlSynch = new object();
        private static readonly object PublishJobSynch = new object();

        public AgentStatus GetAgentStatus()
        {
            try
            {
                lock (AgentControlSynch)
                {
                    if (_agent == null)
                        return new AgentStatus
                        {
                            Status = AgentStatus.AgentStatuses.NotRunning
                        };
                    return _agent.AgentStatus;
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
            }

            return null;
        }

        public void PublishJob(JobDefinition jobDefinition)
        {
            lock (PublishJobSynch)
            {
                using (var agentDbContext = new AgentDbContext())
                {
                    var existingJobDefinition =
                        agentDbContext.JobDefinitions.FirstOrDefault(i =>
                            i.JobDefinitionId == jobDefinition.JobDefinitionId);
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

                if (_agent != null)
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
                lock (AgentControlSynch)
                {
                    if (_agent == null)
                    {
                        var extensionsAssemblyPath = Path.GetFullPath(@"..\..\..\Cloudflow.Extensions\bin\debug\Cloudflow.Extensions.dll");

                        //TODO:This should be injected as a dependency but SignalR makes it difficult to do this in the AgentHub constructor
                        //Using the first method in this article cause an exception whenever the client tried to connect to the Hub
                        //https://docs.microsoft.com/en-us/aspnet/signalr/overview/advanced/dependency-injection
                        using (AgentDbContext agentDbContext = new AgentDbContext(true, extensionsAssemblyPath))
                        {
                            //TODO:Get some jobs
                            var jobs = new List<Job>();
                            var extensionService = new ExtensionService(new JsonConfigurationSerializer());
                            foreach (var jobDefinition in agentDbContext.JobDefinitions)
                            {
                                var jobConfigurationFactory =
                                    new JobConfigurationFactory(jobDefinition, extensionService);
                                jobs.Add(new Job(jobConfigurationFactory.CreateJobConfiguration()));
                            }

                            _agent = new Agent(jobs);
                        }
                    }

                    if (_agent.AgentStatus.Status == AgentStatus.AgentStatuses.NotRunning)
                    {
                        _agent.Start(this);
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
                lock (AgentControlSynch)
                {
                    if (_agent.AgentStatus.Status == AgentStatus.AgentStatuses.Running)
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
                                                      i.Status == Run.RunStatuses.Completed ||
                                                      i.Status == Run.RunStatuses.Failed ||
                                                      i.Status == Run.RunStatuses.Canceled)
                    .OrderByDescending(i => i.DateEnded)
                    .Skip(startIndex).Take(pageSize).ToList();
            }
        }

        public List<Run> GetQueuedRuns()
        {
            lock (AgentControlSynch)
            {
                if (_agent == null)
                    return new List<Run>();
                return null;
            }
        }

        public void OnAgentStarted(IAgent agent)
        {
            Logger.Info("Agent started.");
            Clients.All.updateStatus(agent.AgentStatus);
        }

        public void OnAgentStopped(IAgent agent)
        {
            Logger.Info("Agent stopped.");
            Clients.All.updateStatus(agent.AgentStatus);
        }

        public void OnAgentActivity(IAgent agent, string activity)
        {
            Logger.Info(activity);

            //TODO:Get the run that caused the activity or modify the client side scripting that responds to activity
            //Clients.All.runStatusChanged(run);
        }
    }
}