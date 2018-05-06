using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempProject.Interfaces
{
    public interface IExtensionService
    {
        T CreateNewConfiguration<T>(ICatalogProvider catalogProvider, Guid extensionId);

        T LoadConfiguration<T>(ICatalogProvider catalogProvider, Guid extensionId, string configuration);

        T LoadConfigurableExtension<T>(ICatalogProvider catalogProvider, Guid extensionId, IExtension configuration);
    }
}
