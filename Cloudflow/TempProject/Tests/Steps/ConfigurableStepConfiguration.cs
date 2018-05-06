using System.ComponentModel.Composition;
using TempProject.Steps;

namespace TempProject.Tests.Steps
{
    [Export(typeof(IStepConfiguration))]
    [ExportMetadata("Type", typeof(ConfigurableStepConfiguration))]
    public class ConfigurableStepConfiguration : IStepConfiguration
    {
        public string Message { get; set; }
    }
}