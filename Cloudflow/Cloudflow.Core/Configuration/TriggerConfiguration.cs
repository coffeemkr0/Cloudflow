using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Configuration
{
    public abstract class TriggerConfiguration
    {
        #region Properties
        public string TriggerName { get; }
        #endregion

        #region Constructors
        public TriggerConfiguration(string triggerName)
        {
            this.TriggerName = triggerName;
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
