using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TempProject.Implementations;

namespace TempProject.Tests
{
    [TestClass]
    public class StepExtensionServiceShould
    {
        private StepExtensionService _stepExtensionService;

        [TestInitialize]
        public void InitializeTest()
        {
            _stepExtensionService = new StepExtensionService();
        }

        [TestMethod]
        public void GetHelloWorldStep()
        {
            var step = _stepExtensionService.GetStep(Guid.Parse(HelloWorldStep.ExtensionId));

            Assert.IsNotNull(step);
        }
    }
}
