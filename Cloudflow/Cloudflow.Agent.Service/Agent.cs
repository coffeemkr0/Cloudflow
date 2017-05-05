using Cloudflow.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Cloudflow.Agent.Service
{
    public class Agent
    {
        #region Private Members
        private int _runCounter = 1;
        private List<Task> _runTasks;
        private List<Run> _runs;
        #endregion

        #region Events
        public delegate void StatusChangedEventHandler(AgentStatus status);
        public event StatusChangedEventHandler StatusChanged;
        protected virtual void OnStatusChanged(AgentStatus status)
        {
            StatusChangedEventHandler temp = StatusChanged;
            if (temp != null)
            {
                temp(status);
            }
        }
        #endregion

        #region Properties
        public List<Job> Jobs { get; }

        public TaskScheduler TaskScheduler { get; }

        public log4net.ILog AgentLogger { get; }
        #endregion

        #region Constructors
        public Agent()
        {
            this.AgentLogger = log4net.LogManager.GetLogger("Agent." + Environment.MachineName);

            this.Jobs = new List<Job>();
            _runTasks = new List<Task>();
            _runs = new List<Run>();
        }
        #endregion

        #region Private Methods
        private void Job_JobTriggerFired(Job job, Trigger trigger, Dictionary<string, object> triggerData)
        {
            this.AgentLogger.Info(string.Format("Job trigger fired - Job:{0} Trigger{1}", job.Name, trigger.Name));

            Run run = new Run(string.Format("{0} Run {1}", job.Name, _runCounter++), job, triggerData);
            var task = Task.Run(() =>
            {
                try
                {
                    run.Start();
                }
                catch (Exception ex)
                {
                    this.AgentLogger.Error(ex);
                }
            });

            _runTasks.Add(task);
            _runs.Add(run);
            OnStatusChanged(GetStatus());

            Task.Run(() =>
            {
                task.Wait();
                _runTasks.Remove(task);
                _runs.Remove(run);
                OnStatusChanged(GetStatus());
            });
        }
        #endregion

        #region Public Methods
        public void AddJob(Job job)
        {
            job.JobTriggerFired += Job_JobTriggerFired;
            this.Jobs.Add(job);
        }

        public void Start()
        {
            this.AgentLogger.Info("Starting agent");

            OnStatusChanged(new AgentStatus { Status = AgentStatus.AgentStatuses.Starting });

            foreach (var job in this.Jobs)
            {
                job.Start();
            }

            OnStatusChanged(GetStatus());
        }

        public void Stop()
        {
            this.AgentLogger.Info("Stopping agent");

            foreach (var job in this.Jobs)
            {
                job.Stop();
            }

            this.AgentLogger.Info("Waiting for any runs in progress");
            Task.WaitAll(_runTasks.ToArray());

            this.AgentLogger.Info("Agent stopped");

            OnStatusChanged(new AgentStatus { Status = AgentStatus.AgentStatuses.NotRunning });
        }

        public static Agent CreateTestAgent()
        {
            Agent agent = new Agent();

            agent.AddJob(Job.CreateTestJob("Test Job 1"));
            agent.AddJob(Job.CreateTestJob("Test Job 2"));

            return agent;
        }

        public AgentStatus GetStatus()
        {
            if (_runTasks.Count == 0)
            {
                return new AgentStatus
                {
                    Status = AgentStatus.AgentStatuses.Idle
                };
            }
            else
            {
                var status = new AgentStatus
                {
                    Status = AgentStatus.AgentStatuses.Processing,
                    Runs = string.Join(",", _runs.Select(i => i.Name).ToArray())
                };
                return status;
            }
        }
        #endregion
    }
}
