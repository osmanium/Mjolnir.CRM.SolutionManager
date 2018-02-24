using System.Threading.Tasks;
using CommandLine;
using Mjolnir.ConsoleCommandLine;
using Mjolnir.ConsoleCommandLine.Tracer;
using Mjolnir.CRM.Core;

namespace Mjolnir.CRM.SolutionManager.Operations.CRM
{
    [Verb("Connect-CrmSourceAndTarget")]
    public class ConnectCrmSourceAndTargetCommand : ConsoleCommandBase
    {
        public override async Task<object> ExecuteCommand(ITracingService tracer, object input)
        {
            var sourceCrmContextCommand = new ConnectCrmSourceCommand();
            var sourceCrmContext = (CrmContext)await sourceCrmContextCommand.ExecuteCommand(tracer, input);

            var targetCrmContextCommand = new ConnectCrmTargetCommand();
            var targetCrmContext = (CrmContext)await targetCrmContextCommand.ExecuteCommand(tracer, input);

            if (sourceCrmContext == null || targetCrmContext == null)
                return null;

            return new[]
            {
                sourceCrmContext,
                targetCrmContext
            };
        }
    }
}
