using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TempProject.Implementations;
using TempProject.Interfaces;

namespace TempProject.Tests
{
    [TestClass]
    public class DefaultJobShould
    {
        private JobMonitor _jobMonitor;
        private List<ITrigger> _triggers;
        private List<IStep> _steps;

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
            var job = new DefaultJob(_jobMonitor, _triggers, _steps);
            job.Start();
            Assert.IsTrue(_jobMonitor.OnJobStartedCalled);
        }

        [TestMethod]
        public void Stop()
        {
            var job = new DefaultJob(_jobMonitor, _triggers, _steps);
            job.Start();
            job.Stop();
            Assert.IsTrue(_jobMonitor.OnJobStoppedCalled);
        }

        [TestMethod]
        public void PostActivityWhenTriggerFires()
        {
            var job = new DefaultJob(_jobMonitor, _triggers, _steps);
            job.Start();
            Assert.IsTrue(_jobMonitor.OnJobActivityCalled);
        }

        [TestMethod]
        public void PostActivityWhenStepExecutes()
        {
            var job = new DefaultJob(_jobMonitor, _triggers, _steps);

            job.Start();
            Assert.IsTrue(_jobMonitor.OnJobActivityCalled);
        }

        [TestMethod]
        public void PostActivityWhenStepThrowsException()
        {
            _steps.Add(new ExceptionTestStep());
            var job = new DefaultJob(_jobMonitor, _triggers, _steps);

            job.Start();
            Assert.IsTrue(_jobMonitor.OnJobExceptionCalled);
        }

        [TestMethod]
        public void DisposeAndNotThrowException()
        {
            var job = new DefaultJob(_jobMonitor, _triggers, _steps);
            job.Start();
            job.Stop();
            job.Dispose();
        }
    }
}
