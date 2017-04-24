using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Mjolnir.ConsoleCommandLine;
using Mjolnir.ConsoleCommandLine.InputAttributes;
using Mjolnir.CRM.Core;
using Mjolnir.CRM.SolutionManager.Operations.CRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mjolnir.CRM.SolutionManager.Operations.Entity
{
    [ConsoleCommandAttribute(
        Command = "DeleteEntity",
        Desription = "Deletes given entity(metadata) from customizations",
        DependentCommand = typeof(ConnectCrmSourceCommand))]
    public class DeleteEntityOperation : ConsoleCommandBase
    {
        [StringInput(Description = "Entity scheme name to be deleted.", IsRequired = true)]
        public string EntitySchemeName { get; set; }

        public override object ExecuteCommand(ITracingService tracer, object input)
        {
            try
            {
                tracer.Trace($"Deleting entity {EntitySchemeName}...");

                CrmContext ctx = (CrmContext)input;

                DeleteEntityRequest request = new DeleteEntityRequest()
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
        }
    }
}
