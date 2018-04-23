using System.ComponentModel.Composition;
using TempProject.Interfaces;

namespace TempProject.Tests.Steps
{
    [Export(typeof(IStepConfiguration))]
    [ExportMetadata("ExtensionId", ExtensionId)]
    public class ConfigurableStepConfiguration : IStepConfiguration
    {
        public const string ExtensionId = "{5AA0FAE3-0703-438D-AE86-209ABA558C16}";

        public string Message { get; set; }
    }
}