using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempProject.Interfaces;

namespace TempProject.Implementations
{
    [Export(typeof(IExtension))]
    [ExportMetadata("ExtensionId", ExtensionId)]
    public class DefaultJobConfiguration : IExtension
    {
        public const string ExtensionId = "{B1887673-2E1F-468A-BCA0-975CB043EED6}";

        public List<ITrigger> Triggers { get; set; }

        public List<IStep> Steps { get; set; }

        public DefaultJobConfiguration()
        {
            Triggers = new List<ITrigger>();
            Steps = new List<IStep>();
        }
    }
}
