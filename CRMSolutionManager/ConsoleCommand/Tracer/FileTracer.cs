using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCommandLine.Tracer
{
    public class FileTracer : ITracingService
    {
        public void Trace(string format, params object[] args)
        {
            //TODO : Implement file logging
            throw new NotImplementedException();
        }
    }
}
