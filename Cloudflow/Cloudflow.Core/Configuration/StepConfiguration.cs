using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Configuration
{
    public abstract class StepConfiguration
    {
        #region Properties
        public Guid StepExtensionId { get; }

        public string ExtensionAssemblyPath { get; set; }

        public string StepName { get; set; }
        #endregion

        #region Constructors
        public StepConfiguration(Guid stepExtensionId)
        {
            this.StepExtensionId = stepExtensionId;
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
