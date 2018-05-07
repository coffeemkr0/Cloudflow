using System.ComponentModel.Composition.Primitives;

namespace Cloudflow.Core.ExtensionManagement
{
    public interface ICatalogProvider
    {
        ComposablePartCatalog GetCatalog();
    }
}