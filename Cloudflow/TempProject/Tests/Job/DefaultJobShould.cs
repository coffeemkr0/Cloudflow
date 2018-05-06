using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TempProject.Jobs;
using TempProject.Steps;
using TempProject.Tests.Steps;
using TempProject.Tests.Triggers;
using TempProject.Triggers;

namespace TempProject.Tests.Job
{
    [TestClass]
    public class DefaultJobShould
    {
        private JobMonitor _jobMonitor;
        private JobConfiguration _jobConfiguration;


        [TestInitialize]
        public void InitializeTest()
        {
            _jobMonitor = new JobMonitor();
            _jobConfiguration = new JobConfiguration
            {
                Triggers = new List<ITrigger>
                {
                    new ImmediateTrigger()
                },
                Steps = new List<IStep>
                {
                    new TestStep()
                }
            };
        }

        [TestMethod]
        public void Start()
        {
            var job = new Jobs.Job(_jobConfiguration);
            job.Start(_jobMonitor);
            Assert.IsTrue(_jobMonitor.OnJobStartedCalled);
        }

        [TestMethod]
        public void Stop()
        {
            var job = new Jobs.Job(_jobConfiguration);
            job.Start(_jobMonitor);
            job.Stop();
            Assert.IsTrue(_jobMonitor.OnJobStoppedCalled);
        }

        [TestMethod]
        public void PostActivityWhenTriggerFires()
        {
            var job = new Jobs.Job(_jobConfiguration);
            job.Start(_jobMonitor);
            Assert.IsTrue(_jobMonitor.OnJobActivityCalled);
        }

        [TestMethod]
        public void PostActivityWhenStepExecutes()
        {
            var job = new Jobs.Job(_jobConfiguration);

            job.Start(_jobMonitor);
            Assert.IsTrue(_jobMonitor.OnJobActivityCalled);
        }

        [TestMethod]
        public void PostActivityWhenStepThrowsException()
        {
            _jobConfiguration.Steps.Add(new ExceptionTestStep());
            var job = new Jobs.Job(_jobConfiguration);

            job.Start(_jobMonitor);
            Assert.IsTrue(_jobMonitor.OnJobExceptionCalled);
        }

        [TestMethod]
        public void DisposeAndNotThrowException()
        {
            var job = new Jobs.Job(_jobConfiguration);
            job.Start(_jobMonitor);
            job.Stop();
            job.Dispose();
        }
    }
}