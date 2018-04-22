using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TempProject.Implementations;
using TempProject.Interfaces;

namespace TempProject.Tests
{
    [TestClass]
    public class StepExtensionServiceShould
    {
        private StepExtensionService _stepExtensionService;
        private AssemblyCatalogProvider _assemblyCatalogProvider;

        [TestInitialize]
        public void InitializeTest()
        {
            _assemblyCatalogProvider = new AssemblyCatalogProvider(typeof(HelloWorldStep).Assembly.CodeBase);
            _stepExtensionService = new StepExtensionService(_assemblyCatalogProvider);
        }

        [TestMethod]
        public void GetHelloWorldStep()
        {
            var step = _stepExtensionService.GetStep(Guid.Parse(HelloWorldStep.ExtensionId));

            Assert.AreEqual(step.GetClassName(), "HelloWorldStep");
        }
    }
}
