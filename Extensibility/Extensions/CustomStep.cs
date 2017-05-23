using Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    [Export(typeof(Step))]
    [ExportMetadata("Name", "CustomStep")]
    public class CustomStep : Step
    {
        [ImportingConstructor]
        public CustomStep([Import("StepConfiguration")]StepConfiguration customStepConfiguration) : base(customStepConfiguration)
        {

        }

        public override void Execute()
        {
            Console.WriteLine($"Execute custom step - property value={((CustomStepConfiguration)this.StepConfiguration).CustomStepConfigurationProperty}");
        }
    }
}
