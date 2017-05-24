using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Configuration
{
    public abstract class JobConfiguration
    {
        #region Properties
        public string JobName { get; }

        public List<StepConfiguration> StepConfigurations { get; set; }

        public List<TriggerConfiguration> TriggerConfigurations { get; set; }
        #endregion

        #region Constructors
        public JobConfiguration(string jobName)
        {
            this.JobName = jobName;

            this.StepConfigurations = new List<StepConfiguration>();
            this.TriggerConfigurations = new List<TriggerConfiguration>();
        }
        #endregion

        #region Public Methods
        public void SaveToFile(string fileName)
        {
            using (StreamWriter sw = new StreamWriter(fileName, false))
            {
                sw.WriteLine(JsonConvert.SerializeObject(this));
            }
        }

        public static object LoadFromFile(Type type, string fileName)
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                string content = sr.ReadToEnd();
                return JsonConvert.DeserializeObject(content, type);
            }
        }
        #endregion
    }
}
