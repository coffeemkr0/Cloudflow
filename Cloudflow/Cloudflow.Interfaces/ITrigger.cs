using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Interfaces
{
    public interface ITrigger
    {
        void Initialize();

        event EventHandler Triggered;
    }
}
