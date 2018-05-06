using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempProject.Implementations
{
    public class ExtensionNotFoundException : Exception
    {
        public ExtensionNotFoundException(Guid extensionId) : base(
            $"An extension with the Id {extensionId} could not be found.")
        {

        }
    }
}
