using ConsoleCommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace CRMSolutionManager.CLI.Commands.Solution
{
    [ConsoleCommandAttribute(
        Command = "ConvertPatchToSolution",
        Desription = "",
        DependentCommand = typeof(CrmConnectCommand))]
    public class ConvertPatchToSolutionCommand : ConsoleCommandBase
    {
        public override object Execute(ITracingService tracer, object input)
        {
            return true;
        }
    }
}
