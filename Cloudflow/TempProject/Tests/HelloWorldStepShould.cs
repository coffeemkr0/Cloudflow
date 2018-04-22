using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TempProject.Implementations;
using TempProject.Interfaces;

namespace TempProject.Tests
{
    public class StepMonitor : IStepMonitor
    {
        public bool OnCompletedCalled { get; set; }
        public bool OnStartedCalled { get; set; }
        public bool OnActivityCalled { get; set; }
        public bool OnStepDisposingCalled { get; set; }

        public void OnStepCompleted(IStep step)
        {
            OnCompletedCalled = true;
            Console.WriteLine($"[{step.GetClassName()}] Step completed");
        }

        public void OnStepStarted(IStep step)
        {
            OnStartedCalled = true;
            Console.WriteLine($"[{step.GetClassName()}] Step started");
        }

        public void OnStepActivity(IStep step, string activity)
        {
            OnActivityCalled = true;
            Console.WriteLine($"[{step.GetClassName()}] Step activity - {activity}");
        }

        public void OnStepDisposing(IStep step)
        {
            OnStepDisposingCalled = true;
            Console.WriteLine($"[{step.GetClassName()}] Disposing");
        }
    }

    [TestClass]
    public class HelloWorldStepShould
    {
        private HelloWorldStep _helloWorldStep;
        private StepMonitor _stepMonitor;

        [TestInitialize]
        public void InitializeTest()
        {
            _stepMonitor = new StepMonitor();
            _helloWorldStep = new HelloWorldStep(_stepMonitor, "Hello World!");
        }

        [TestMethod]
        public void ExecuteAndStart()
        {
            _helloWorldStep.Execute();
            Assert.IsTrue(_stepMonitor.OnStartedCalled);
        }

        [TestMethod]
        public void ExecuteAndComplete()
        {
            _helloWorldStep.Execute();
            Assert.IsTrue(_stepMonitor.OnCompletedCalled);
        }

        [TestMethod]
        public void ExecuteAndPostActivity()
        {
            _helloWorldStep.Execute();
            Assert.IsTrue(_stepMonitor.OnActivityCalled);
        }

        [TestMethod]
        public void CallDisposing()
        {
            _helloWorldStep.Dispose();
            Assert.IsTrue(_stepMonitor.OnStepDisposingCalled);
        }
    }
}