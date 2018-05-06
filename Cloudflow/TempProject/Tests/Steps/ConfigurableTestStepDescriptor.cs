using System;
using System.ComponentModel.Composition;
using TempProject.Steps;

namespace TempProject.Tests.Steps
{
    [Export(typeof(IStepDescriptor))]
    [ExportMetadata("ExtensionId", Id)]
    public class ConfigurableTestStepDescriptor : IStepDescriptor
    {
        public const string Id = "{FB3EC0D5-4918-4B20-81AA-BD64864048E5}";

        public Guid ExtensionId => Guid.Parse(Id);

        public Type ExtensionType => typeof(ConfigurableTestStep);

        public string Name => "Configurable Test Step";

        public string Description => "A test step with a simple configuration.";

        public Type ConfigurationType => typeof(ConfigurableStepConfiguration);

        public byte[] Icon => null;
    }
}