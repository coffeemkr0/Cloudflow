using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core
{
    public class Step
    {
        #region Properties
        public Guid Id { get; }

        public string Name { get; }
        #endregion

        #region Constructors
        public Step(string name)
        {
            this.Id = Guid.NewGuid();
            this.Name = name;
        }
        #endregion
    }
}
