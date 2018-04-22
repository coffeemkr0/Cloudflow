using System;
using System.Collections.Generic;
using TempProject.Interfaces;

namespace TempProject.Implementations
{
    public class DefaultJob : IJob, ITriggerMonitor, IStepMonitor
    {
        private IJobMonitor _jobMonitor;
        private readonly IEnumerable<IStep> _steps;
        private readonly IEnumerable<ITrigger> _triggers;

        public DefaultJob(IEnumerable<ITrigger> triggers, IEnumerable<IStep> steps)
        {
            _triggers = triggers;
            _steps = steps;
        }

        public void Stop()
        {
            foreach (var trigger in _triggers) trigger.Stop();

            _jobMonitor.OnJobStopped(this);
        }

        public void Dispose()
        {
            foreach (var trigger in _triggers) trigger.Dispose();
        }

        public void Start(IJobMonitor jobMonitor)
        {
            _jobMonitor = jobMonitor;

            foreach (var trigger in _triggers) trigger.Start(this);

            _jobMonitor.OnJobStarted(this);
        }

        public void OnStepStarted(IStep step)
        {
            _jobMonitor.OnJobActivity(this, "Step started");
        }

        public void OnStepActivity(IStep step, string activity)
        {
            _jobMonitor.OnJobActivity(this, $"Step activity - {activity}");
        }

        public void OnStepCompleted(IStep step)
        {
            _jobMonitor.OnJobActivity(this, "Step completed");
        }

        public void OnTriggerFired(ITrigger trigger)
        {
            _jobMonitor.OnJobActivity(this, "Trigger fired");

            foreach (var step in _steps)
            {
                try
                {
                    step.Execute(this);
                }
                catch (Exception e)
                {
                    _jobMonitor.OnException(this, e);
                }
            }
        }

        public void OnTriggerStarted(ITrigger trigger)
        {
            _jobMonitor.OnJobActivity(this, "Trigger started");
        }

        public void OnTriggerStopped(ITrigger trigger)
        {
            _jobMonitor.OnJobActivity(this, "Trigger stopped");
        }
    }
}