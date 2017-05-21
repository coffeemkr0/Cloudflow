using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public abstract class Step
    {
        public string Name { get; set; }

        public abstract void Execute();
    }
}
