using Cloudflow.Core.Configuration;
using Cloudflow.Core.Data.Agent.Models;
using Cloudflow.Core.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Runtime
{
    public class JobController
    {
        #region Events
        public delegate void RunStatusChangedEventHandler(Run run);
        public event RunStatusChangedEventHandler RunStatusChanged;
        protected virtual void OnRunStatusChanged(Run run)
        {
            RunStatusChangedEventHandler temp = RunStatusChanged;
            if (temp != null)
            {
                temp(run);
            }
        }

        public delegate void StepOutputEventHandler(Job job, Step step, OutputEventLevels level, string message);
        public event StepOutputEventHandler StepOutput;
        protected virtual void OnStepOutput(Job job, Step step, OutputEventLevels level, string message)
        {
            StepOutputEventHandler temp = StepOutput;
            if (temp != null)
            {
                temp(job, step, level, message);
            }
        }
        #endregion

        #region Private Members
        [ImportMany]
        IEnumerable<Lazy<Job, IJobMetaData>> _jobs = null;
        private CompositionContainer _jobsContainer;

        private int _runCounter = 1;
        private List<RunController> _runControllers;
        private List<Task> _runTasks;
        #endregion

        #region Properties
        public JobConfiguration JobConfiguration { get; }

        public Job Job { get; }

        public log4net.ILog JobControllerLoger { get; }
        #endregion

        #region Constructors
        public JobController(JobConfiguration jobConfiguration)
        {
            this.JobConfiguration = jobConfiguration;
            this.JobControllerLoger = log4net.LogManager.GetLogger($"JobController.{jobConfiguration.JobName}");

            _runControllers = new List<RunController>();
            _runTasks = new List<Task>();

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(@"..\..\..\Cloudflow.Extensions\bin\debug\Cloudflow.Extensions.dll"));
            _jobsContainer = new CompositionContainer(catalog);
            _jobsContainer.ComposeExportedValue<JobConfiguration>("JobConfiguration", jobConfiguration);

            try
            {
                _jobsContainer.ComposeParts(this);

                foreach (Lazy<Job, IJobMetaData> i in _jobs)
                {
                    if (Guid.Parse(i.Metadata.JobExtensionId) == this.JobConfiguration.JobExtensionId)
                    {
                        i.Value.TriggerFired += Job_TriggerFired;
                        i.Value.StepOutput += Job_StepOutput;
                        this.Job = i.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                this.JobControllerLoger.Error(ex);
            }
        }
        #endregion

        #region Private Methods
        private void Job_TriggerFired(Job job, Trigger trigger, Dictionary<string, object> triggerData)
        {
            this.JobControllerLoger.Info("Trigger event accepted - creating a run controller");
            RunController runController = new RunController(string.Format("{0} Run {1}", job.JobConfiguration.JobName, _runCounter++), job, triggerData);
            runController.RunStatusChanged += RunController_RunStatusChanged;

            var task = Task.Run(() =>
            {
                try
                {
                    this.JobControllerLoger.Info("Executing run");
                    runController.ExecuteRun();
                }
                catch (Exception ex)
                {
                    this.JobControllerLoger.Error(ex);
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

        private void Job_StepOutput(Job job, Step step, OutputEventLevels level, string message)
        {
            OnStepOutput(job, step, level, message);
        }

        private void RunController_RunStatusChanged(Run run)
        {
            OnRunStatusChanged(run);
        }
        #endregion

        #region Public Methods
        public void Start()
        {
            this.Job.Start();
        }

        public void Stop()
        {
            this.Job.Stop();
        }

        public void Wait()
        {
            Task.WaitAll(_runTasks.ToArray());
        }

        public List<Run> GetQueuedRuns()
        {
            lock (_runControllers)
            {
                return _runControllers.Select(i => i.Run).ToList();
            }
        }
        #endregion
    }
}
