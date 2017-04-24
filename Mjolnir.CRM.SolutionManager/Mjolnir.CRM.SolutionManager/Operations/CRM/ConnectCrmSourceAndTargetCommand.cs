using Mjolnir.ConsoleCommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Mjolnir.CRM.Core;

namespace Mjolnir.CRM.SolutionManager.Operations.CRM
{
    [ConsoleCommandAttribute(
        Command = "ConnectCrmSourceAndTarget",
        DependentCommand = typeof(ConnectCrmSourceCommand),
        Desription = "")]
    public class ConnectCrmSourceAndTargetCommand : ConsoleCommandBase
    {
        public override object ExecuteCommand(ITracingService tracer, object input)
        {
            var sourceCrmContext = input as CrmContext;
            var targetCrmContext = new ConnectCrmTargetCommand().ExecuteCommand(tracer, input) as CrmContext;

            if (sourceCrmContext == null || targetCrmContext == null)
                return null;

            return new CrmContext[]
            {
                sourceCrmContext,
                targetCrmContext
            };
        }
    }
}
