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
            _extensionService = new ExtensionService();
        }

        [TestMethod]
        public void ReturnNullForInvalidExtensionId()
        {
            var extension =
                _extensionService.LoadConfigurableExtension<IExtension>(_assemblyCatalogProvider, Guid.Empty, null);

            Assert.IsNull(extension);
        }

        [TestMethod]
        public void GetTestStepThatExecutes()
        {
            var step = _extensionService.LoadConfigurableExtension<IStep>(_assemblyCatalogProvider,
                Guid.Parse(TestStep.ExtensionId), null);
            step.Execute(new StepMonitor());
            Assert.AreEqual(step.GetClassName(), typeof(TestStep).Name);
        }

        [TestMethod]
        public void GetConfigurableTestStepThatExecutes()
        {
            var configuration = new ConfigurableStepConfiguration {Message = "Test configuration"};
            var step = _extensionService.LoadConfigurableExtension<IStep>(_assemblyCatalogProvider,
                Guid.Parse(ConfigurableTestStep.ExtensionId), configuration);
            step.Execute(new StepMonitor());
            Assert.AreEqual(step.GetClassName(), typeof(ConfigurableTestStep).Name);
        }
    }
}