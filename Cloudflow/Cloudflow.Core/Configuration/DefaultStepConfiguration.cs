using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Configuration
{
    public class DefaultStepConfiguration : StepConfiguration
    {
        #region Public Methods
        public static DefaultStepConfiguration CreateTestStepConfiguration(string name)
        {
            return new DefaultStepConfiguration()
            {
                Name = name
            };
        }
        #endregion
    }
}
