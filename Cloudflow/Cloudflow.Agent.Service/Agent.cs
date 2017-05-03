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
        }
        #endregion

        #region Private Methods
        private void LoadJobs()
        {
            this.AgentLogger.Info("Loading jobs");

            this.Jobs.Clear();
            var job = Job.CreateTestJob("Test Job 1");
            job.JobTriggerFired += Job_JobTriggerFired;
            this.Jobs.Add(job);

            job = Job.CreateTestJob("Test Job 2");
            job.JobTriggerFired += Job_JobTriggerFired;
            this.Jobs.Add(job);
        }

        private void Job_JobTriggerFired(Job job, Trigger trigger, Dictionary<string, object> triggerData)
        {
            this.AgentLogger.Info(string.Format("Job trigger fired - Job:{0} Trigger{1}", job.Name, trigger.Name));

            _runTasks.Add(Task.Run(() =>
            {
                try
                {
                    Run run = new Run(string.Format("{0} Run {1}", job.Name, _runCounter++), job, triggerData);
                    run.Start();
                }
                catch (Exception ex)
                {
                    this.AgentLogger.Error(ex);
                }
            }));
        }
        #endregion

        #region Public Methods
        public void Start()
        {
            _runTasks = new List<Task>();

            LoadJobs();
            foreach (var job in this.Jobs)
            {
                job.Start();
            }
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
        }
        #endregion
    }
}
