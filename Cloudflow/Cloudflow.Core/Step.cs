﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core
{
    public class Step
    {
        private static readonly log4net.ILog _logger =
               log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Public Methods
        public void Execute(Dictionary<string, object> triggerData)
        {
            _logger.Info("Executing the step");
            using (StreamWriter sw = new StreamWriter(@"JobOutput\Outputfile.txt", true))
            {
                string output = string.Format("{0}[Step] Hello World", DateTime.Now.ToString());
                _logger.Debug(string.Format("Writing output to file - {0}", output));
                sw.WriteLine(output);
            }
        }
        #endregion
    }
}