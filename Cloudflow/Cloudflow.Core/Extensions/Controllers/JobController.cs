using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Threading.Tasks;
using Cloudflow.Core.Agents;
using Cloudflow.Core.Data.Agent.Models;
using Cloudflow.Core.Data.Shared.Models;
using log4net;

namespace Cloudflow.Core.Extensions.Controllers
{
    public class JobController : IJobController, IJobControllerData
    {
        [ImportMany]
        private IEnumerable<Lazy<IConfigurableExtension, IConfigurableExtensionMetaData>> _extensions = null;

        private readonly IJobMonitor _jobMonitor;

        private readonly CompositionContainer _jobsContainer;
        private readonly List<RunController> _runControllers;
        private readonly List<Task> _runTasks;

        private int _runCounter = 1;

        public JobController(JobDefinition jobDefinition, IJobMonitor jobMonitor)
        {
            JobDefinition = jobDefinition;
            _jobMonitor = jobMonitor;

            _runControllers = new List<RunController>();
            _runTasks = new List<Task>();

            //Load the job configuration
            var jobConfigurationController = new ExtensionConfigurationController(
                jobDefinition.ConfigurationExtensionId,
                jobDefinition.ConfigurationExtensionAssemblyPath);
            JobConfiguration = jobConfigurationController.Load(jobDefinition.Configuration);

            //Create the logger for the controller
            JobControllerLoger = LogManager.GetLogger($"JobController.{JobConfiguration.Name}");

            //Load the triggers
            TriggerControllers = new List<TriggerController>();
            foreach (var triggerDefinition in jobDefinition.TriggerDefinitions)
            {
                var triggerController = new TriggerController(triggerDefinition);
                triggerController.TriggerFired += TriggerController_TriggerFired;
                TriggerControllers.Add(triggerController);
            }

            //Load the steps
            StepControllers = new List<StepController>();
            foreach (var stepDefinition in jobDefinition.StepDefinitions)
            {
                var stepController = new StepController(stepDefinition);
                stepController.StepOutput += StepController_StepOutput;
                StepControllers.Add(stepController);
            }

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(jobDefinition.ExtensionAssemblyPath));
            _jobsContainer = new CompositionContainer(catalog);
            _jobsContainer.ComposeExportedValue("ExtensionConfiguration", JobConfiguration);

            try
            {
                _jobsContainer.ComposeParts(this);

                foreach (var i in _extensions)
                    if (Guid.Parse(i.Metadata.ExtensionId) == JobDefinition.ExtensionId)
                        Job = (Job) i.Value;
            }
            catch (Exception ex)
            {
                JobControllerLoger.Error(ex);
            }
        }

        public JobDefinition JobDefinition { get; }

        public ExtensionConfiguration JobConfiguration { get; }

        public Job Job { get; }

        public List<TriggerController> TriggerControllers { get; }

        public List<StepController> StepControllers { get; }

        public ILog JobControllerLoger { get; }

        public void Start()
        {
            JobControllerLoger.Info("Starting the job");
            try
            {
                Job.Start();

                foreach (var triggerController in TriggerControllers) triggerController.Start();
            }
            catch (Exception ex)
            {
                JobControllerLoger.Error(ex);
            }
        }

        public void Stop()
        {
            JobControllerLoger.Info("Stopping the job");

            try
            {
                foreach (var triggerController in TriggerControllers) triggerController.Stop();

                Job.Stop();
            }
            catch (Exception ex)
            {
                JobControllerLoger.Error(ex);
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

        private void TriggerController_TriggerFired(Trigger trigger)
        {
            JobControllerLoger.Info("Trigger event accepted - creating a run controller");
            var runController = new RunController(string.Format("{0} Run {1}",
                JobConfiguration.Name, _runCounter++), this);
            runController.RunStatusChanged += RunController_RunStatusChanged;

            var task = Task.Run(() =>
            {
                try
                {
                    JobControllerLoger.Info("Executing run");
                    runController.ExecuteRun();
                }
                catch (Exception ex)
                {
                    JobControllerLoger.Error(ex);
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
            _jobMonitor.StepOutput(Job, step, level, message);
        }

        private void RunController_RunStatusChanged(Run run)
        {
            _jobMonitor.RunStatusChanged(run);
        }
    }
}