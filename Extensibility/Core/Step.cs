using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public abstract class Step
    {
        public StepConfiguration StepConfiguration { get; }

        public Step(StepConfiguration stepConfiguration)
        {
            this.StepConfiguration = stepConfiguration;
        }

        public abstract void Execute();
    }
}
