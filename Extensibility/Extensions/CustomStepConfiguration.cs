using Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    [Export(typeof(StepConfiguration))]
    [ExportMetadata("Name", "CustomStep")]
    public class CustomStepConfiguration : StepConfiguration
    {
        public string CustomStepConfigurationProperty { get; set; }

        public CustomStepConfiguration() : base("CustomStep")
        {
            this.CustomStepConfigurationProperty = "Hello World from CustomStep Configuration!";
        }
    }
}
