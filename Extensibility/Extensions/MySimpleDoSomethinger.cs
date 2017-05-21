using Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    [Export(typeof(IDoSomething))]
    public class MySimpleDoSomethinger : IDoSomething
    {
        public void DoSomething()
        {
            Console.WriteLine("Hello world from MySimpleCalculator.DoSomething!");
        }
    }
}
