using System.ComponentModel.Composition;
using TempProject.Interfaces;

namespace TempProject.Tests.Steps
{
    [Export(typeof(IExtension))]
    [ExportMetadata("ExtensionId", ExtensionId)]
    [ExportMetadata("ExtensionType", typeof(ConfigurableStepConfiguration))]
    public class ConfigurableStepConfiguration : IExtension
    {
        public const string ExtensionId = "{B62A0AA1-B220-449A-85DC-C31E4FCABA0D}";

        public string Message { get; set; }
    }
}