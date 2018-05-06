using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempProject.Interfaces;

namespace TempProject.Tests.Steps
{
    [Export(typeof(IStepDescriptor))]
    [ExportMetadata("ExtensionId", Id)]
    public class TestStepDescriptor: IStepDescriptor
    {
        public const string Id = "{5AA0FAE3-0703-438D-AE86-209ABA558C16}";

        public Guid ExtensionId => Guid.Parse(Id);

        public Type ExtensionType => typeof(TestStep);

        public string Name => "Test Step";

        public string Description => "A simple test step.";

        public Type ConfigurationType => null;

        public byte[] Icon => null;
    }
}
