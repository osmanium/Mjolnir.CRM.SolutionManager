using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Mjolnir.ConsoleCommandLine;
using Mjolnir.CRM.Core;
using Mjolnir.CRM.SolutionManager.Operations.CRM;
using System;
using System.Threading.Tasks;
using CommandLine;

namespace Mjolnir.CRM.SolutionManager.Operations.Entity
{
    [Verb("Delete-Entity")]
    public class DeleteEntityOperation : ConsoleCommandBase
    {
        [Option('e',"entityschemaname",
            Required = true,
            HelpText = "Entity scheme name to be deleted.")]
        public string EntitySchemeName { get; set; }

        public override async Task<object> ExecuteCommand(ConsoleCommandLine.Tracer.ITracingService tracer, object input)
        {
            return Task.Run(() =>
            {
                try
                {
                    tracer.Trace($"Deleting entity {EntitySchemeName}...");

                    var ctx = (CrmContext)input;

                    var request = new DeleteEntityRequest()
                    {
                        LogicalName = EntitySchemeName,
                    };

                    ctx.OrganizationService.Execute(request);

                    tracer.Trace($"Successfully deleted entity {EntitySchemeName}...");
                    return true;
                }
                catch (Exception ex)
                {
                    HandleCommandException(tracer, ex);
                    return false;
                }
            });
        }
    }
}
