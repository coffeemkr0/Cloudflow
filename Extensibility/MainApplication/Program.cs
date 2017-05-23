using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var configController = new StepConfigurationController();
            var customStepConfiguration = configController.GetStepConfiguration("CustomStep");
            var stepController = new StepController(customStepConfiguration);
            stepController.Step.Execute();

            customStepConfiguration = configController.GetStepConfiguration("CustomStep2");
            stepController = new StepController(customStepConfiguration);
            stepController.Step.Execute();

            Console.ReadLine();
        }
    }
}
