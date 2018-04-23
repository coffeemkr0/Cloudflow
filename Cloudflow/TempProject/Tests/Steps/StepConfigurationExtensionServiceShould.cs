using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TempProject.Implementations;
using TempProject.Interfaces;

namespace TempProject.Tests.Steps
{
    [TestClass]
    public class StepConfigurationExtensionServiceShould
    {
        private AssemblyCatalogProvider _assemblyCatalogProvider;
        private StepConfigurationExtensionService _stepConfigurationExtensionService;

        [TestInitialize]
        public void InitializeTest()
        {
            _assemblyCatalogProvider = new AssemblyCatalogProvider(GetType().Assembly.CodeBase);
            _stepConfigurationExtensionService = new StepConfigurationExtensionService(_assemblyCatalogProvider);
        }

        [TestMethod]
        public void ReturnNullForInvalidExtensionId()
        {
            var configuration = _stepConfigurationExtensionService.GetConfiguration(Guid.Empty);

            Assert.IsNull(configuration);
        }

        [TestMethod]
        public void GetConfigurableStepConfiguration()
        {
            var configuration =
                _stepConfigurationExtensionService.GetConfiguration(
                    Guid.Parse(ConfigurableStepConfiguration.ExtensionId));
            Assert.AreEqual(configuration.GetClassName(), typeof(ConfigurableStepConfiguration).Name);
        }
    }
}