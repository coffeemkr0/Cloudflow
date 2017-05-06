using Cloudflow.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Cloudflow.Core.Data.Agent;

namespace Cloudflow.Agent.Service
{
    public class Agent
    {
        #region Private Members
        private int _runCounter = 1;
        private List<Task> _runTasks;
        private List<RunController> _runControllers;
        #endregion

        #region Events
        public delegate void StatusChangedEventHandler(AgentStatus status);
        public event StatusChangedEventHandler StatusChanged;
        protected virtual void OnStatusChanged()
        {
            StatusChangedEventHandler temp = StatusChanged;
            if (temp != null)
            {
                temp(this.AgentStatus);
            }
        }
        #endregion

        #region Properties
        public List<Job> Jobs { get; }

        public TaskScheduler TaskScheduler { get; }

        public log4net.ILog AgentLogger { get; }

        private AgentStatus _agentStatus;

        public AgentStatus AgentStatus
        {
            get { return _agentStatus; }
            set
            {
                if (_agentStatus != value)
                {
                    _agentStatus = value;
                    OnStatusChanged();
                }
            }
        }
        #endregion

        #region Constructors
        public Agent()
        {
            this.AgentLogger = log4net.LogManager.GetLogger("Agent." + Environment.MachineName);

            this.Jobs = new List<Job>();
            _runTasks = new List<Task>();
            _runControllers = new List<RunController>();
            this.AgentStatus = new AgentStatus { Status = AgentStatus.AgentStatuses.NotRunning };
        }
        #endregion

        #region Private Methods
        private void Job_JobTriggerFired(Job job, Trigger trigger, Dictionary<string, object> triggerData)
        {
            this.AgentLogger.Info(string.Format("Job trigger fired - Job:{0} Trigger{1}", job.Name, trigger.Name));

            RunController runController = new RunController(string.Format("{0} Run {1}", job.Name, _runCounter++), job, triggerData);

            var task = Task.Run(() =>
            {
                try
                {
                    runController.ExecuteRun();
                }
                catch (Exception ex)
                {
                    this.AgentLogger.Error(ex);
                }
            });

            _runTasks.Add(task);
            _runControllers.Add(runController);

            Task.Run(() =>
            {
                task.Wait();
                _runTasks.Remove(task);
                _runControllers.Remove(runController);
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

            this.AgentStatus = new AgentStatus { Status = AgentStatus.AgentStatuses.Starting };

            foreach (var job in this.Jobs)
            {
                job.Start();
            }


            this.AgentStatus = new AgentStatus { Status = AgentStatus.AgentStatuses.Running };
        }

        public void Stop()
        {
            this.AgentLogger.Info("Stopping agent");

            this.AgentStatus = new AgentStatus { Status = AgentStatus.AgentStatuses.Stopping };

            foreach (var job in this.Jobs)
            {
                job.Stop();
            }

            this.AgentLogger.Info("Waiting for any runs in progress");
            Task.WaitAll(_runTasks.ToArray());

            this.AgentLogger.Info("Agent stopped");
            this.AgentStatus = new AgentStatus { Status = AgentStatus.AgentStatuses.NotRunning };
        }

        public static Agent CreateTestAgent()
        {
            Agent agent = new Agent();

            agent.AddJob(Job.CreateTestJob("Test Job 1"));
            agent.AddJob(Job.CreateTestJob("Test Job 2"));

            return agent;
        }
        #endregion
    }
}
