using Microsoft.VisualStudio.TestTools.UnitTesting;
using TempProject.Implementations;

namespace TempProject.Tests
{
    [TestClass]
    public class HelloWorldStepShould
    {
        private HelloWorldStep _helloWorldStep;
        private StepMonitor _stepMonitor;

        [TestInitialize]
        public void InitializeTest()
        {
            _stepMonitor = new StepMonitor();
            _helloWorldStep = new HelloWorldStep("Hello World!");
        }

        [TestMethod]
        public void ExecuteAndStart()
        {
            _helloWorldStep.Execute(_stepMonitor);
            Assert.IsTrue(_stepMonitor.OnStartedCalled);
        }

        [TestMethod]
        public void ExecuteAndComplete()
        {
            _helloWorldStep.Execute(_stepMonitor);
            Assert.IsTrue(_stepMonitor.OnCompletedCalled);
        }

        [TestMethod]
        public void ExecuteAndPostActivity()
        {
            _helloWorldStep.Execute(_stepMonitor);
            Assert.IsTrue(_stepMonitor.OnActivityCalled);
        }

        [TestMethod]
        public void DisposeAndNotThrowException()
        {
            _helloWorldStep.Execute(_stepMonitor);
            _helloWorldStep.Dispose();
        }
    }
}