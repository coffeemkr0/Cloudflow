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
        public Guid JobExtensionId { get; }

        public string ExtensionAssemblyPath { get; set; }

        public string JobName { get; set; }
        #endregion

        #region Constructors
        public JobConfiguration(Guid jobExtensionId)
        {
            this.JobExtensionId = jobExtensionId;
        }
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
