using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Agent.Service.Data.Models
{
    public class Run
    {
        #region Properties
        public int Id { get; set; }

        public string Name { get; set; }

        public string JobName { get; set; }

        public DateTime DateStarted { get; set; }

        public DateTime? DateCompleted { get; set; }
        #endregion
    }
}
