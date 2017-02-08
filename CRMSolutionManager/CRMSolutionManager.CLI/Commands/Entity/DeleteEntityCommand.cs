using Common;
using ConsoleCommandLine;
using ConsoleCommandLine.InputAttributes;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSolutionManager.CLI.Commands.Entity
{
    [ConsoleCommandAttribute(
        Command = "DeleteEntity",
        Desription = "",
        DependentCommand = typeof(CrmConnectCommand))]
    public class DeleteEntityCommand : ConsoleCommandBase
    {
        [StringInput(Description = "Entity scheme name to be deleted.", IsRequired = true)]
        public string EntitySchemeName { get; set; }

        public override object Execute(ITracingService tracer, object input)
        {
            try
            {
                tracer.Trace($"Deleting entity {EntitySchemeName}...");

                CRMContext ctx = (CRMContext)input;

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
