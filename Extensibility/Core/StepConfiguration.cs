using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public abstract class StepConfiguration
    {
        #region Properties
        public string StepName { get; }
        #endregion

        #region Constructors
        public StepConfiguration(string stepName)
        {
            this.StepName = stepName;
        }
        #endregion
    }
}
