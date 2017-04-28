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
        #region Public Methods
        public void Execute(Dictionary<string, object> triggerData)
        {
            using (StreamWriter sw = new StreamWriter(@"JobOutput\Outputfile.txt", true))
            {
                string output = string.Format("{0}[Step] Hello World", DateTime.Now.ToString());
                Console.WriteLine("Writing output - " + output);
                sw.WriteLine(output);
            }
        }
        #endregion
    }
}
