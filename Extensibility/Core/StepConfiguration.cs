using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
