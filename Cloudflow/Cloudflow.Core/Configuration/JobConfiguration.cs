using Cloudflow.Core.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Configuration
{
    public abstract class JobConfiguration : IExtension
    {
        #region Properties
        public Type Type { get; }

        public string Id { get; }

        public Guid JobExtensionId { get; set; }

        public string JobExtensionAssemblyPath { get; set; }

        public string JobName { get; set; }
        #endregion

        #region Constructors
        protected JobConfiguration()
        {
            var exportExtensionAttribute = this.GetType().GetCustomAttributes(typeof(ExportExtensionAttribute), true).
                Cast<ExportExtensionAttribute>()
                .SingleOrDefault();

            if(exportExtensionAttribute!= null)
            {
                this.Id = exportExtensionAttribute.Id;
                this.Type = exportExtensionAttribute.Type;
            }
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
