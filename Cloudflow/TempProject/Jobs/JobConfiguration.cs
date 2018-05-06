using System.Collections.Generic;
using TempProject.Steps;
using TempProject.Triggers;

namespace TempProject.Jobs
{
    public class JobConfiguration
    {
        public JobConfiguration()
        {
            Triggers = new List<ITrigger>();
            Steps = new List<IStep>();
        }

        public string Name { get; set; }

        public List<ITrigger> Triggers { get; set; }

        public List<IStep> Steps { get; set; }
    }
}