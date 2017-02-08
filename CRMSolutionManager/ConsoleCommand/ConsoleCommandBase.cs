using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCommandLine
{
    public abstract class ConsoleCommandBase
    {
        public abstract object Execute(ITracingService tracer, object input);

        public virtual string GetHelp()
        {
            //TODO : Iterate properties with types
            return null;
        }

        public void HandleCommandException(ITracingService tracer, Exception ex)
        {
            tracer.Trace(ex.Message + "\n" + ex.StackTrace + "\n");
        }
    }
}
