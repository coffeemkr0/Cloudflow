using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Runtime
{
    public delegate void StepOutputEventHandler(Step step, OutputEventLevels level, string message);
}
