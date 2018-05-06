using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempProject.Interfaces
{
    public interface IExtensionService
    {
        T GetExtension<T>(Guid extensionId);
    }
}
