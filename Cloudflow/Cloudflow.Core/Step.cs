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
        private static Random _rand = new Random();

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
            this.StepLogger.Info("Hello World from inside the test step's code!");

            System.Threading.Thread.Sleep(_rand.Next(1, 3) * 1000);
        }
        #endregion
    }
}
