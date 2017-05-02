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

        public log4net.ILog StepLogger { get; }
        #endregion

        #region Constructors
        public Step(string name)
        {
            this.StepLogger = log4net.LogManager.GetLogger("Step." + name);

            this.Id = Guid.NewGuid();
            this.Name = name;
        }
        #endregion

        #region Public Methods
        public void Execute(Dictionary<string, object> triggerData)
        {
            this.StepLogger.Info("Executing the step");
            if (!Directory.Exists("JobOutput"))
            {
                Directory.CreateDirectory("JobOutput");
            }
            using (StreamWriter sw = new StreamWriter(@"JobOutput\Outputfile.txt", true))
            {
                string output = string.Format("{0}[Step] Hello World", DateTime.Now.ToString());
                this.StepLogger.Debug(string.Format("Writing output to file - {0}", output));
                sw.WriteLine(output);
            }
        }
        #endregion
    }
}
