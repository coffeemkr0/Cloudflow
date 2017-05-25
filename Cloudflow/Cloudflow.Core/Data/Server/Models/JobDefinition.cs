using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Data.Server.Models
{
    public class JobDefinition
    {
        #region Properties
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid JobExtensionId { get; set; }

        public string JobExtensionAssemblyPath { get; set; }
        #endregion
    }
}
