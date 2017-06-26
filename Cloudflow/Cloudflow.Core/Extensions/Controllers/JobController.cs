﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Threading.Tasks;
using Cloudflow.Core.Data.Agent.Models;
using Cloudflow.Core.Data.Shared.Models;
using Cloudflow.Core.Runtime;

namespace Cloudflow.Core.Extensions.Controllers
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
        IEnumerable<Lazy<IConfigurableExtension, IConfigurableExtensionMetaData>> _extensions = null;
        private CompositionContainer _jobsContainer;

        private int _runCounter = 1;
        private List<RunController> _runControllers;
        private List<Task> _runTasks;
        #endregion

        #region Properties
        public JobDefinition JobDefinition { get; }

        public ExtensionConfiguration JobConfiguration { get; }

        public Job Job { get; }

        public List<TriggerController> TriggerControllers { get; }

        public List<StepController> StepControllers { get; }

        public log4net.ILog JobControllerLoger { get; }
        #endregion

        #region Constructors
        public JobController(JobDefinition jobDefinition)
        {
            this.JobDefinition = jobDefinition;

            _runControllers = new List<RunController>();
            _runTasks = new List<Task>();

            //Load the job configuration
            var jobConfigurationController = new ExtensionConfigurationController(jobDefinition.ConfigurationExtensionId,
                jobDefinition.ConfigurationExtensionAssemblyPath);
            this.JobConfiguration = jobConfigurationController.Load(jobDefinition.Configuration);

            //Create the logger for the controller
            this.JobControllerLoger = log4net.LogManager.GetLogger($"JobController.{this.JobConfiguration.Name}");

            //Load the triggers
            this.TriggerControllers = new List<TriggerController>();
            foreach (var triggerDefinition in jobDefinition.TriggerDefinitions)
            {
                var triggerController = new TriggerController(triggerDefinition);
                triggerController.TriggerFired += TriggerController_TriggerFired;
                this.TriggerControllers.Add(triggerController);
            }

            //Load the steps
            this.StepControllers = new List<StepController>();
            foreach (var stepDefinition in jobDefinition.StepDefinitions)
            {
                var stepController = new StepController(stepDefinition);
                stepController.StepOutput += StepController_StepOutput;
                this.StepControllers.Add(stepController);
            }

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(jobDefinition.ExtensionAssemblyPath));
            _jobsContainer = new CompositionContainer(catalog);
            _jobsContainer.ComposeExportedValue<ExtensionConfiguration>("ExtensionConfiguration", this.JobConfiguration);

            try
            {
                _jobsContainer.ComposeParts(this);

                foreach (Lazy<IConfigurableExtension, IConfigurableExtensionMetaData> i in _extensions)
                {
                    if (Guid.Parse(i.Metadata.ExtensionId) == this.JobDefinition.ExtensionId)
                    {
                        this.Job = (Job)i.Value;
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
        private void TriggerController_TriggerFired(Trigger trigger, Dictionary<string, object> triggerData)
        {
            this.JobControllerLoger.Info("Trigger event accepted - creating a run controller");
            RunController runController = new RunController(string.Format("{0} Run {1}",
                this.JobConfiguration.Name, _runCounter++), this, triggerData);
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

        private void StepController_StepOutput(Step step, OutputEventLevels level, string message)
        {
            OnStepOutput(this.Job, step, level, message);
        }

        private void RunController_RunStatusChanged(Run run)
        {
            OnRunStatusChanged(run);
        }
        #endregion

        #region Public Methods
        public void Start()
        {
            this.JobControllerLoger.Info("Starting the job");
            try
            {
                this.Job.Start();

                foreach (var triggerController in this.TriggerControllers)
                {
                    triggerController.Start();
                }
            }
            catch (Exception ex)
            {
                this.JobControllerLoger.Error(ex);
            }
        }

        public void Stop()
        {
            this.JobControllerLoger.Info("Stopping the job");

            try
            {
                foreach (var triggerController in this.TriggerControllers)
                {
                    triggerController.Stop();
                }

                this.Job.Stop();
            }
            catch (Exception ex)
            {
                this.JobControllerLoger.Error(ex);
            }
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
