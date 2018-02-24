using Mjolnir.ConsoleCommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Xrm.Sdk;
using Mjolnir.CRM.JavaScriptOperation;
using Mjolnir.CRM.Core;
using Mjolnir.CRM.SolutionManager.Operations.CRM;

namespace Mjolnir.CRM.SolutionManager.Operations.Solution
{
    [Verb("Apply-SolutionUpgrade")]
    public class PublishAllOperation : ConsoleCommandBase
    {
        [Option('s',"solutionuniquename",
            Required = true,
            HelpText = "Solution uniuq name to be upgraded.")]
        public string SelectedSolutionUniqueName { get; set; }
        

        public override async Task<object> ExecuteCommand(ConsoleCommandLine.Tracer.ITracingService tracer, object input)
        {
            //CrmContext ctx = input as CrmContext;
            //return new Mjolnir.CRM.SolutionManager.BusinessManagers.SolutionBusiness().PublishAll(new PublishAllRequest(), null, ctx);
            return Task.Run(() => true);
        }
    }
}
