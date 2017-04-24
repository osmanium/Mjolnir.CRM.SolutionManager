using Mjolnir.ConsoleCommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Mjolnir.ConsoleCommandLine.InputAttributes;
using Mjolnir.CRM.JavaScriptOperation;
using Mjolnir.CRM.Core;
using Mjolnir.CRM.SolutionManager.Operations.CRM;

namespace Mjolnir.CRM.SolutionManager.Operations.Solution
{

    [ConsoleCommandAttribute(
        Command = "ApplySolutionUpgrade",
        Desription = "",
        DependentCommand = typeof(ConnectCrmSourceCommand))]
    public class PublishAllOperation : ConsoleCommandBase
    {
        [StringInput(Description = "Solution uniuq name to be upgraded.", IsRequired = true)]
        public string SelectedSolutionUniqueName { get; set; }


        public override object ExecuteCommand(ITracingService tracer, object input)
        {
            //CrmContext ctx = input as CrmContext;
            //return new Mjolnir.CRM.SolutionManager.BusinessManagers.SolutionBusiness().PublishAll(new PublishAllRequest(), null, ctx);
            return false;
        }
    }
}
