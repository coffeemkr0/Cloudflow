using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempProject.Interfaces;

namespace TempProject.Implementations
{
    public class AssemblyCatalogProvider : ICatalogProvider
    {
        private readonly string _assemblyPath;

        public AssemblyCatalogProvider(string assemblyPath)
        {
            _assemblyPath = assemblyPath;
        }

        public ComposablePartCatalog GetCatalog()
        {
            return new AssemblyCatalog(_assemblyPath);
        }
    }
}
