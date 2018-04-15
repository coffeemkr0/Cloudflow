namespace Cloudflow.Core.Extensions
{
    /// <summary>
    ///     A base class that an extension can inherit from to implement an object factory for
    ///     the creation of new objects.
    /// </summary>
    public abstract class ObjectFactory : Extension
    {
        #region Constructors

        public ObjectFactory(string factoryData)
        {
            FactoryData = factoryData;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the data that was used to instantiate the factory.
        /// </summary>
        public string FactoryData { get; }

        #endregion

        #region Public Methods

        public abstract object CreateObject(string instanceData);

        #endregion
    }
}