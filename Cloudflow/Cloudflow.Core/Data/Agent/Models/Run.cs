using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Data.Agent.Models
{
    public class Run
    {
        #region Properties
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string JobName { get; set; }

        public DateTime DateStarted { get; set; }

        public DateTime? DateCompleted { get; set; }
        #endregion

        #region Constructors
        public Run()
        {
            this.Id = Guid.NewGuid();
        }
        #endregion
    }
}
