using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TempProject.Implementations;
using TempProject.Interfaces;
using TempProject.Tests.Steps;

namespace TempProject.Tests
{
    [TestClass]
    public class ExtensionServiceShould
    {
        private AssemblyCatalogProvider _assemblyCatalogProvider;
        private ExtensionService _extensionService;

        [TestInitialize]
        public void InitializeTest()
        {
            _assemblyCatalogProvider = new AssemblyCatalogProvider(GetType().Assembly.CodeBase);
        }

        [TestMethod]
        public void ReturnNullForInvalidExtensionId()
        {
            _extensionService = new ExtensionService(_assemblyCatalogProvider, null);
            var extension = _extensionService.GetExtension(Guid.Empty);

            Assert.IsNull(extension);
        }

        [TestMethod]
        public void GetTestStepThatExecutes()
        {
            _extensionService = new ExtensionService(_assemblyCatalogProvider, null);
            var step = (IStep)_extensionService.GetExtension(Guid.Parse(TestStep.ExtensionId));
            step.Execute(new StepMonitor());
            Assert.AreEqual(step.GetClassName(), typeof(TestStep).Name);
        }

        [TestMethod]
        public void GetConfigurableTestStepThatExecutes()
        {
            var configuration = new ConfigurableStepConfiguration {Message = "Test configuration"};
            _extensionService = new ExtensionService(_assemblyCatalogProvider, configuration);
            var step = (IStep)_extensionService.GetExtension(Guid.Parse(ConfigurableTestStep.ExtensionId));
            step.Execute(new StepMonitor());
            Assert.AreEqual(step.GetClassName(), typeof(ConfigurableTestStep).Name);
        }
    }
}