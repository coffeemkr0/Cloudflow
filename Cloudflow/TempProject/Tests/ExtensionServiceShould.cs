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
        public void ThrowsCorrectExceptionForInvalidTriggerId()
        {
            bool correctExceptionThrown = false;
            try
            {
                var extension =
                    _extensionService.LoadTrigger(_assemblyCatalogProvider, Guid.Empty, null);
            }
            catch (ExtensionNotFoundException e)
            {
                Assert.IsNotNull(e);
                correctExceptionThrown = true;
            }

            Assert.IsTrue(correctExceptionThrown);
        }

        [TestMethod]
        public void GetTestStepThatExecutes()
        {
            var step = _extensionService.LoadStep(_assemblyCatalogProvider,
                Guid.Parse(TestStepDescriptor.Id), null);
            step.Execute(new StepMonitor());
            Assert.AreEqual(step.GetClassName(), typeof(TestStep).Name);
        }

        [TestMethod]
        public void GetConfigurableTestStepThatExecutes()
        {
            var configuration = new ConfigurableStepConfiguration {Message = "Test configuration"};
            var step = _extensionService.LoadStep(_assemblyCatalogProvider,
                Guid.Parse(ConfigurableTestStepDescriptor.Id), configuration);
            step.Execute(new StepMonitor());
            Assert.AreEqual(step.GetClassName(), typeof(ConfigurableTestStep).Name);
        }
    }
}