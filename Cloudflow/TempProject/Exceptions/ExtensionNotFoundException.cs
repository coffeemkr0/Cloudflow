using System;

namespace TempProject.Exceptions
{
    public class ExtensionNotFoundException : Exception
    {
        public ExtensionNotFoundException(Guid extensionId) : base(
            $"An extension with the Id {extensionId} could not be found.")
        {

        }
    }
}
