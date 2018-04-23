using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempProject.Interfaces
{
    public interface IExtensionService
    {
        IExtension GetExtension(string extensionId);

        IJob GetJob(string extensionId);

        ITrigger GetTrigger(string extensionId);

        IStep GetStep(string extensionId);
    }
}
