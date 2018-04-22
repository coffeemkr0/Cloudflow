using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TempProject.Implementations;
using TempProject.Interfaces;
using TempProject.Tests.Steps;
using TempProject.Tests.Triggers;

namespace TempProject.Tests.Job
{
    [TestClass]
    public class DefaultJobShould
    {
        private JobMonitor _jobMonitor;
        private List<IStep> _steps;
        private List<ITrigger> _triggers;

        [TestInitialize]
        public void InitializeTest()
        {
            _jobMonitor = new JobMonitor();

            _triggers = new List<ITrigger>
            {
                new ImmediateTrigger()
            };
            _steps = new List<IStep>
            {
                new TestStep()
            };
        }

        [TestMethod]
        public void Start()
        {
            var job = new DefaultJob(_triggers, _steps);
            job.Start(_jobMonitor);
            Assert.IsTrue(_jobMonitor.OnJobStartedCalled);
        }

        [TestMethod]
        public void Stop()
        {
            var job = new DefaultJob(_triggers, _steps);
            job.Start(_jobMonitor);
            job.Stop();
            Assert.IsTrue(_jobMonitor.OnJobStoppedCalled);
        }

        [TestMethod]
        public void PostActivityWhenTriggerFires()
        {
            var job = new DefaultJob(_triggers, _steps);
            job.Start(_jobMonitor);
            Assert.IsTrue(_jobMonitor.OnJobActivityCalled);
        }

        [TestMethod]
        public void PostActivityWhenStepExecutes()
        {
            var job = new DefaultJob(_triggers, _steps);

            job.Start(_jobMonitor);
            Assert.IsTrue(_jobMonitor.OnJobActivityCalled);
        }

        [TestMethod]
        public void PostActivityWhenStepThrowsException()
        {
            _steps.Add(new ExceptionTestStep());
            var job = new DefaultJob(_triggers, _steps);

            job.Start(_jobMonitor);
            Assert.IsTrue(_jobMonitor.OnJobExceptionCalled);
        }

        [TestMethod]
        public void DisposeAndNotThrowException()
        {
            var job = new DefaultJob(_triggers, _steps);
            job.Start(_jobMonitor);
            job.Stop();
            job.Dispose();
        }
    }
}