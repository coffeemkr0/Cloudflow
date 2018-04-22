using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TempProject.Implementations;
using TempProject.Interfaces;

namespace TempProject.Tests.Steps
{
    [TestClass]
    public class StepExtensionServiceShould
    {
        private AssemblyCatalogProvider _assemblyCatalogProvider;
        private StepExtensionService _stepExtensionService;

        [TestInitialize]
        public void InitializeTest()
        {
            _assemblyCatalogProvider = new AssemblyCatalogProvider(this.GetType().Assembly.CodeBase);
        }

        [TestMethod]
        public void ReturnNullForInvalidExtensionId()
        {
            _stepExtensionService = new StepExtensionService(_assemblyCatalogProvider, null);
            var step = _stepExtensionService.GetStep(Guid.Empty);

            Assert.IsNull(step);
        }

        [TestMethod]
        public void GetTestStepThatExecutes()
        {
            _stepExtensionService = new StepExtensionService(_assemblyCatalogProvider, null);
            var step = _stepExtensionService.GetStep(Guid.Parse(TestStep.ExtensionId));
            step.Execute(new StepMonitor());
            Assert.AreEqual(step.GetClassName(), typeof(TestStep).Name);
        }

        [TestMethod]
        public void GetConfigurableTestStepThatExecutes()
        {
            _stepExtensionService = new StepExtensionService(_assemblyCatalogProvider, "Test configuration");
            var step = _stepExtensionService.GetStep(Guid.Parse(ConfigurableTestStep.ExtensionId));
            step.Execute(new StepMonitor());
            Assert.AreEqual(step.GetClassName(), typeof(ConfigurableTestStep).Name);
        }
    }
}