namespace Cloudflow.Core.Extensions
{
    /// <summary>
    /// A base class that an extension can inherit from to implement an object factory for
    /// the creation of new objects.
    /// </summary>
    public abstract class ObjectFactory : Extension
    {
        #region Properties
        /// <summary>
        /// Gets the data that was used to instantiate the factory.
        /// </summary>
        public string FactoryData { get; private set; }
        #endregion

        #region Constructors
        public ObjectFactory(string factoryData) : base()
        {
            FactoryData = factoryData;
        }
        #endregion

        #region Public Methods
        public abstract object CreateObject(string instanceData);
        #endregion
    }
}
