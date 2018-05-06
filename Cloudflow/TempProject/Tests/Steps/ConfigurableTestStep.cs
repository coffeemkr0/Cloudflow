using System.ComponentModel.Composition;
using TempProject.Interfaces;

namespace TempProject.Tests.Steps
{
    [Export(typeof(IStep))]
    [ExportMetadata("Type", typeof(ConfigurableTestStep))]
    public class ConfigurableTestStep : IStep
    {
        private readonly ConfigurableStepConfiguration _configuration;

        private IStepMonitor _stepMonitor;

        [ImportingConstructor]
        public ConfigurableTestStep([Import("Configuration")] IStepConfiguration configuration)
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