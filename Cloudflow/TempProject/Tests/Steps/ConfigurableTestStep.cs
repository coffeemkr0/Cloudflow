using System.ComponentModel.Composition;
using TempProject.Interfaces;

namespace TempProject.Tests.Steps
{
    [Export(typeof(IStep))]
    [ExportMetadata("ExtensionId", ExtensionId)]
    public class ConfigurableTestStep : IStep
    {
        public const string ExtensionId = "{FB3EC0D5-4918-4B20-81AA-BD64864048E5}";

        private IStepMonitor _stepMonitor;
        private readonly string _message;

        [ImportingConstructor]
        public ConfigurableTestStep([Import("Configuration")] object configuration)
        {
            _message = configuration.ToString();
        }

        public void Dispose()
        {
        }

        public void Execute(IStepMonitor stepMonitor)
        {
            _stepMonitor = stepMonitor;

            _stepMonitor.OnStepStarted(this);

            _stepMonitor.OnStepActivity(this, _message);

            _stepMonitor.OnStepCompleted(this);
        }
    }
}
