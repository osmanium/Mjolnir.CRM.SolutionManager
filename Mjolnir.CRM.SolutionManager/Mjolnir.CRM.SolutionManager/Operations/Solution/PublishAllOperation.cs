using Mjolnir.ConsoleCommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Mjolnir.ConsoleCommandLine.InputAttributes;
using Mjolnir.CRM.JavaScriptOperation;
using Mjolnir.CRM.SolutionManager.Infrastructure.PublishAll;
using Mjolnir.CRM.Core;
using Mjolnir.CRM.SolutionManager.Operations.CRM;

namespace Mjolnir.CRM.SolutionManager.Operations.Solution
{

    [ConsoleCommandAttribute(
        Command = "ApplySolutionUpgrade",
        Desription = "",
        DependentCommand = typeof(CrmConnectCommand))]
    public class PublishAllOperation : JavaScriptOperationBase<PublishAllRequest, PublishAllResponse>
    {
        [StringInput(Description = "Solution uniuq name to be upgraded.", IsRequired = true)]
        public string SelectedSolutionUniqueName { get; set; }


        public override object ExecuteCommand(ITracingService tracer, object input)
        {
            throw new NotImplementedException();
        }

        public override PublishAllResponse ExecuteJavascriptOperation(PublishAllRequest req, PublishAllResponse res, CrmContext context)
        {
            return new Mjolnir.CRM.SolutionManager.BusinessManagers.SolutionBusiness().PublishAll(req, res, context); ;
        }
    }
}
