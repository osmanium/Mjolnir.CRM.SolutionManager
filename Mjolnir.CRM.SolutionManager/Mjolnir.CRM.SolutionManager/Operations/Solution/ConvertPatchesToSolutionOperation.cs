using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using Mjolnir.ConsoleCommandLine;
using Mjolnir.CRM.JavaScriptOperation;
using Mjolnir.CRM.SolutionManager.Infrastructure.ConvertPatchesToSolution;
using Mjolnir.CRM.Core;
using Mjolnir.CRM.SolutionManager.Operations.CRM;
using ITracingService = Mjolnir.ConsoleCommandLine.Tracer.ITracingService;

namespace Mjolnir.CRM.SolutionManager.Operations.Solution
{
    [Verb("Convert-PatchToSolution")]
    public class ConvertPatchesToSolutionOperation : JavaScriptOperationBase<ConvertPatchesToSolutionRequest, ConvertPatchesToSolutionResponse>
    {
        [Option('s', "solutionuniquename",
            Required = true,
            HelpText = "Solution unique name to be merged with patches.")]
        public string SelectedSolutionUniqueName { get; set; }

        public override async Task<object> ExecuteCommand(ITracingService tracer, object input)
        {
            var sourceCrmContextCommand = new ConnectCrmSourceCommand();
            var sourceCrmContext        = (CrmContext)await sourceCrmContextCommand.ExecuteCommand(tracer, input);

            var solutionId = new Core.EntityManagers.SolutionManager(sourceCrmContext).GetSolutionIdByUniqueSolutionName(SelectedSolutionUniqueName);
            if (solutionId != Guid.Empty)
            {
                var req = new ConvertPatchesToSolutionRequest()
                {
                    SelectedSolutionIds = new [] { solutionId.ToString() }
                };

                return new Mjolnir.CRM.SolutionManager.BusinessManagers.SolutionBusiness().ConvertPatchToSolution(req, new ConvertPatchesToSolutionResponse(), sourceCrmContext); ;
            }
            else
            {
                Console.WriteLine($"Solution with name : {SelectedSolutionUniqueName} not found.");
                return false;
            }
        }

        public override ConvertPatchesToSolutionResponse ExecuteJavascriptOperation(ConvertPatchesToSolutionRequest  req,
                                                                                    ConvertPatchesToSolutionResponse res, CrmContext context)
        {
            return new Mjolnir.CRM.SolutionManager.BusinessManagers.SolutionBusiness().ConvertPatchToSolution(req, res, context); ;
        }
    }
}