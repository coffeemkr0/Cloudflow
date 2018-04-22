using System;
using System.Timers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TempProject.Implementations;
using TempProject.Interfaces;

namespace TempProject.Tests
{
    public class TriggerMonitor : ITriggerMonitor
    {
        public bool OnDisposedCalled { get; set; }
        public bool OnFiredCalled { get; set; }
        public bool OnStartCalled { get; set; }
        public bool OnStopCalled { get; set; }

        public void OnTriggerDisposed(ITrigger trigger)
        {
            OnDisposedCalled = true;
            Console.WriteLine($"[{trigger.GetClassName()}] Trigger disposed");
        }

        public void OnTriggerFired(ITrigger trigger)
        {
            OnFiredCalled = true;
            Console.WriteLine($"[{trigger.GetClassName()}] Trigger fired");
        }

        public void OnTriggerStarted(ITrigger trigger)
        {
            OnStartCalled = true;
            Console.WriteLine($"[{trigger.GetClassName()}] Trigger started");
        }

        public void OnTriggerStopped(ITrigger trigger)
        {
            OnStopCalled = true;
            Console.WriteLine($"[{trigger.GetClassName()}] Trigger stopped");
        }
    }

    public class TestTimer : ITimer
    {
        public double Interval { get; set; }

        public event ElapsedEventHandler Elapsed;

        public void Dispose()
        {
        }

        public void Start()
        {
            Elapsed?.Invoke(this, null);
        }

        public void Stop()
        {
        }
    }

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
            _timerTrigger = new TimerTrigger(_timer, _triggerMonitor);
        }


        [TestMethod]
        public void Start()
        {
            _timerTrigger.Start();
            Assert.IsTrue(_triggerMonitor.OnStartCalled);
        }

        [TestMethod]
        public void StartAndFire()
        {
            _timerTrigger.Start();
            Assert.IsTrue(_triggerMonitor.OnFiredCalled);
        }

        [TestMethod]
        public void Stop()
        {
            _timerTrigger.Start();
            _timerTrigger.Stop();
            Assert.IsTrue(_triggerMonitor.OnStopCalled);
        }

        [TestMethod]
        public void DisposeAndStop()
        {
            _timerTrigger.Start();
            _timerTrigger.Dispose();
            Assert.IsTrue(_triggerMonitor.OnStopCalled);
            Assert.IsTrue(_triggerMonitor.OnDisposedCalled);
        }
    }
}