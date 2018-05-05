using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempProject.Interfaces
{
    public interface IExtensionService
    {
        IExtension GetExtension(Guid extensionId);

        IJob GetJob(Guid extensionId);

        ITrigger GetTrigger(Guid extensionId);

        IStep GetStep(Guid extensionId);
    }
}
