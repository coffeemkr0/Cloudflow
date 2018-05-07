using System.ComponentModel.Composition;
using TempProject.Steps;

namespace TempProject.Tests.Steps
{
    [Export(typeof(IStep))]
    [ExportMetadata("Type", typeof(ConfigurableTestStep))]
    public class ConfigurableTestStep : IStep
    {
        private readonly ConfigurableStepConfiguration _configuration;

        [ImportingConstructor]
        public ConfigurableTestStep([Import("Configuration")] IStepConfiguration configuration)
        {
            _configuration = (ConfigurableStepConfiguration) configuration;
        }

        public void Dispose()
        {
        }

        public void Execute(IStepMonitor stepMonitor)
        {
            stepMonitor.OnStepStarted(this);

            stepMonitor.OnStepActivity(this, _configuration.Message);

            stepMonitor.OnStepCompleted(this);
        }
    }
}