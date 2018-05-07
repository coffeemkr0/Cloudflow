using System;
using System.ComponentModel.Composition;
using System.Drawing;
using Cloudflow.Core.Steps;
using Cloudflow.Extensions.Properties;

namespace Cloudflow.Extensions.Steps
{
    [Export(typeof(IStepDescriptor))]
    [ExportMetadata("ExtensionId", Id)]
    public class LogStepDescriptor : IStepDescriptor
    {
        public const string Id = "{43D6FD16-0344-4204-AEE9-A09B3998C017}";

        public Guid ExtensionId => Guid.Parse(Id);

        public Type ExtensionType => typeof(LogStep);

        public string Name => Resources.LogStepName;

        public string Description => Resources.LogStepDescription;

        public Type ConfigurationType => typeof(LogStepConfiguration);

        public Image Icon => null;
    }
}