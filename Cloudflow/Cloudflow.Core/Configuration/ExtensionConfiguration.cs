using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Configuration
{
    public class ExtensionConfiguration : Extension
    {
        #region Properties
        public Guid ExtensionId { get; set; }

        public string ExtensionAssemblyPath { get; set; }

        public string Name { get; set; }
        #endregion

        #region Public Methods
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static object Load(Type type, string json)
        {
            return JsonConvert.DeserializeObject(json, type);
        }
        #endregion
    }
}
