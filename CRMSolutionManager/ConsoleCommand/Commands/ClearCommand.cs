using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCommandLine.Commands
{
    [ConsoleCommandAttribute(
        Command = "Clear",
        Desription = "")]
    public class ClearCommand : ConsoleCommandBase
    {
        public override object Execute(ITracingService tracer, object input)
        {
            Console.Clear();
            return true;
        }
    }
}
