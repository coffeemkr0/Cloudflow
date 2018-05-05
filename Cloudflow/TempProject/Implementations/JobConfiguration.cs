using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempProject.Interfaces;

namespace TempProject.Implementations
{
    public class JobConfiguration
    {
        public List<ITrigger> Triggers { get; set; }

        public List<IStep> Steps { get; set; }

        public JobConfiguration()
        {
            Triggers = new List<ITrigger>();
            Steps = new List<IStep>();
        }
    }
}
