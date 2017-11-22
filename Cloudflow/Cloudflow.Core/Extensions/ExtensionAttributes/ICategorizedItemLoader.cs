using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Extensions.ExtensionAttributes
{
    public interface ICategorizedItemFetcher
    {
        CategorizedItemCollection GetItems(string propertyName);
    }
}
