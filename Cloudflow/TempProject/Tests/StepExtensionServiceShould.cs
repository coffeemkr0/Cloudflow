using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TempProject.Implementations;
using TempProject.Interfaces;

namespace TempProject.Tests
{
    [TestClass]
    public class StepExtensionServiceShould
    {
        private AssemblyCatalogProvider _assemblyCatalogProvider;
        private StepExtensionService _stepExtensionService;

        [TestInitialize]
        public void InitializeTest()
        {
            _assemblyCatalogProvider = new AssemblyCatalogProvider(typeof(HelloWorldStep).Assembly.CodeBase);
            _stepExtensionService = new StepExtensionService(_assemblyCatalogProvider);
        }

        [TestMethod]
        public void ReturnNullForInvalidExtensionId()
        {
            var step = _stepExtensionService.GetStep(Guid.Empty);

            Assert.IsNull(step);
        }

        [TestMethod]
        public void GetHelloWorldStep()
        {
            var step = _stepExtensionService.GetStep(Guid.Parse(HelloWorldStep.ExtensionId));

            Assert.AreEqual(step.GetClassName(), "HelloWorldStep");
        }

        [TestMethod]
        public void GetTestStep()
        {
            var step = _stepExtensionService.GetStep(Guid.Parse(TestStep.ExtensionId));

            Assert.AreEqual(step.GetClassName(), "TestStep");
        }
    }
}