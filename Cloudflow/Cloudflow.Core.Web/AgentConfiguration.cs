using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Web
{
    public class AgentConfiguration
    {
        public int Id { get; set; }

        public bool Enabled { get; set; }

        public string MachineName { get; set; }
    }
}
