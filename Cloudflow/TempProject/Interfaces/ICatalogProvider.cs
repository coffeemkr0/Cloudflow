using System.ComponentModel.Composition.Primitives;

namespace TempProject.Interfaces
{
    public interface ICatalogProvider
    {
        ComposablePartCatalog GetCatalog();
    }
}