using Cloudflow.Core.Data.Agent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Runtime
{
    public delegate void RunOutputEventHandler(Run run, OutputEventLevels level, string message);
}
