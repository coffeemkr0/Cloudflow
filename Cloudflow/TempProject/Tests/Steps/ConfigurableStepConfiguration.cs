using System.ComponentModel.Composition;
using TempProject.Interfaces;

namespace TempProject.Tests.Steps
{
    [Export(typeof(IStepConfiguration))]
    [ExportMetadata("Type", typeof(ConfigurableStepConfiguration))]
    public class ConfigurableStepConfiguration : IStepConfiguration
    {
        public string Message { get; set; }
    }
}