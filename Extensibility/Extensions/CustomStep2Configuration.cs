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
    [ExportMetadata("Name", "CustomStep2")]
    [ExportMetadata("Type", typeof(CustomStep2Configuration))]
    public class CustomStep2Configuration : StepConfiguration
    {
        public string CustomStep2ConfigurationProperty { get; set; }

        public CustomStep2Configuration() : base("CustomStep2")
        {
            this.CustomStep2ConfigurationProperty = "Hello World from CustomStep2 Configuration!";
        }
    }
}
