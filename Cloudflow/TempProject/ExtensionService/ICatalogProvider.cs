using System.ComponentModel.Composition.Primitives;

namespace TempProject.ExtensionService
{
    public interface ICatalogProvider
    {
        ComposablePartCatalog GetCatalog();
    }
}