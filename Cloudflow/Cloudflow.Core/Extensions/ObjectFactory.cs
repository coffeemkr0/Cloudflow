using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Extensions
{
    public abstract class ObjectFactory : Extension
    {
        #region Properties
        public string FactoryData { get; private set; }
        #endregion

        #region Constructors
        public ObjectFactory(string factoryData) : base()
        {
            this.FactoryData = factoryData;
        }
        #endregion

        #region Public Methods
        public abstract object CreateObject(string instanceData);
        #endregion
    }
}
