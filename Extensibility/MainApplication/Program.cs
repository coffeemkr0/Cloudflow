using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            MainApp app = new MainApp();
            app.Start();
            app.ExecuteStep("CustomStep");
            app.ExecuteStep("CustomStep2");

            Console.ReadLine();
        }
    }
}
