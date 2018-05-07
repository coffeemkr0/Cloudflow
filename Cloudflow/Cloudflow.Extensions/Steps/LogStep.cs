using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.ExtensionAttributes;
using System;
using System.ComponentModel.Composition;
using Cloudflow.Core.Agents;
using Cloudflow.Core.Steps;

namespace Cloudflow.Extensions.Steps
{
    [Export(typeof(IStep))]
    [ExportMetadata("Type", typeof(LogStep))]
    public class LogStep : IStep
    {
        private readonly LogStepConfiguration _configuration;

        [ImportingConstructor]
        public LogStep([Import("Configuration")] IStepConfiguration configuration)
        {
            _configuration = (LogStepConfiguration) configuration;
        }

        public void Dispose()
        {
            
        }

        public void Execute(IStepMonitor stepMonitor)
        {
            stepMonitor.OnStepActivity(this, _configuration.LogMessage);
        }
    }
}
