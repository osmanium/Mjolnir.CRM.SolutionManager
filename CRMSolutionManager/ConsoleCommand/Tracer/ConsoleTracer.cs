using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCommandLine.Tracer
{
    public class ConsoleTracer : ITracingService
    {
        public void Trace(string format, params object[] args)
        {
            Console.Write(DateTime.Now.ToShortTimeString() + " - ");
            Console.WriteLine(format, args);
        }
    }
}
