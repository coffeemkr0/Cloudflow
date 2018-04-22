using Microsoft.VisualStudio.TestTools.UnitTesting;
using TempProject.Implementations;

namespace TempProject.Tests.Triggers
{
    [TestClass]
    public class TimerTriggerShould
    {
        private TestTimer _timer;
        private TimerTrigger _timerTrigger;
        private TriggerMonitor _triggerMonitor;

        [TestInitialize]
        public void InitializeTest()
        {
            _timer = new TestTimer {Interval = 0};
            _triggerMonitor = new TriggerMonitor();
            _timerTrigger = new TimerTrigger(_timer);
        }


        [TestMethod]
        public void Start()
        {
            _timerTrigger.Start(_triggerMonitor);
            Assert.IsTrue(_triggerMonitor.OnStartCalled);
        }

        [TestMethod]
        public void StartAndFire()
        {
            _timerTrigger.Start(_triggerMonitor);
            Assert.IsTrue(_triggerMonitor.OnFiredCalled);
        }

        [TestMethod]
        public void Stop()
        {
            _timerTrigger.Start(_triggerMonitor);
            _timerTrigger.Stop();
            Assert.IsTrue(_triggerMonitor.OnStopCalled);
        }

        [TestMethod]
        public void DisposeAndStop()
        {
            _timerTrigger.Start(_triggerMonitor);
            _timerTrigger.Dispose();
            Assert.IsTrue(_triggerMonitor.OnStopCalled);
        }
    }
}