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
            var configController = new StepConfigurationController("CustomStep");
            var customStepConfiguration = configController.CreateNewConfiguration();
            customStepConfiguration.SaveToFile("CustomStepConfiguration.txt");

            customStepConfiguration = configController.LoadFromFile("CustomStepConfiguration.txt");
            var stepController = new StepController(customStepConfiguration);
            stepController.Step.Execute();

            var config2Controller = new StepConfigurationController("CustomStep2");
            var customStep2Configuration = config2Controller.CreateNewConfiguration();
            customStep2Configuration.SaveToFile("CustomStep2Configuration.txt");

            customStep2Configuration = config2Controller.LoadFromFile("CustomStep2Configuration.txt");
            var step2Controller = new StepController(customStep2Configuration);
            step2Controller.Step.Execute();

            Console.ReadLine();
        }
    }
}
