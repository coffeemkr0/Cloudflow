using System.ComponentModel.Composition;
using TempProject.Interfaces;

namespace TempProject.Tests.Steps
{
    [Export(typeof(IExtension))]
    [ExportMetadata("ExtensionId", ExtensionId)]
    [ExportMetadata("ExtensionType", typeof(ConfigurableTestStep))]
    public class ConfigurableTestStep : IStep, IExtension
    {
        public const string ExtensionId = "{FB3EC0D5-4918-4B20-81AA-BD64864048E5}";
        private readonly ConfigurableStepConfiguration _configuration;

        private IStepMonitor _stepMonitor;

        [ImportingConstructor]
        public ConfigurableTestStep([Import("Configuration")] IExtension configuration)
        {
            _configuration = (ConfigurableStepConfiguration)configuration;
        }

        public void Dispose()
        {
        }

        public void Execute(IStepMonitor stepMonitor)
        {
            _stepMonitor = stepMonitor;

            _stepMonitor.OnStepStarted(this);

            _stepMonitor.OnStepActivity(this, _configuration.Message);

            _stepMonitor.OnStepCompleted(this);
        }
    }
}