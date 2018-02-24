using Mjolnir.CRM.Core;
using Mjolnir.CRM.Core.EntityManagers;
using Mjolnir.CRM.JavaScriptOperation;
using Mjolnir.CRM.SolutionManager.BusinessManagers;
using Mjolnir.CRM.SolutionManager.Infrastructure;
using Mjolnir.CRM.SolutionManager.Infrastructure.ApplySolutionUpgrade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using Mjolnir.ConsoleCommandLine.Tracer;
using Mjolnir.CRM.SolutionManager.Operations.CRM;

namespace Mjolnir.CRM.SolutionManager.Operations.Solution
{
    [Verb("Apply-SolutionUpgrade")]
    public class ApplySolutionUpgradeOperation : JavaScriptOperationBase<ApplySolutionUpgradeRequest, ApplySolutionUpgradeResponse>
    {
        [Option('s', "solutionuniquename",
            Required = true,
            HelpText = "Solution uniuq name to be upgraded.")]
        public string SelectedSolutionUniqueName { get; set; }

        public override async Task<object> ExecuteCommand(ITracingService tracer, object input)
        {
            var sourceCrmContextCommand = new ConnectCrmSourceCommand();
            var sourceCrmContext        = (CrmContext)await sourceCrmContextCommand.ExecuteCommand(tracer, input);

            var solutionId = new Core.EntityManagers.SolutionManager(sourceCrmContext).GetSolutionIdByUniqueSolutionName(SelectedSolutionUniqueName);

            if (solutionId != Guid.Empty)
            {
                var req = new ApplySolutionUpgradeRequest()
                {
                    SelectedSolutionIds = new string[] { solutionId.ToString() }
                };

                return new Mjolnir.CRM.SolutionManager.BusinessManagers.SolutionBusiness().ApplySolutionUpgrade(req, new ApplySolutionUpgradeResponse(), sourceCrmContext); ;
            }
            else
            {
                Console.WriteLine($"Solution with name : {SelectedSolutionUniqueName} not found.");
                return false;
            }
        }

        public override ApplySolutionUpgradeResponse ExecuteJavascriptOperation(ApplySolutionUpgradeRequest req,
                                                                                ApplySolutionUpgradeResponse res, CrmContext context)
        {
            return new SolutionBusiness().ApplySolutionUpgrade(req, res, context);
        }
    }
}
