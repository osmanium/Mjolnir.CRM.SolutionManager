using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Mjolnir.ConsoleCommandLine;
using Mjolnir.ConsoleCommandLine.InputAttributes;
using Mjolnir.CRM.Common;
using Mjolnir.CRM.SolutionManager.Infrastructure.ConvertPatchesToSolution;

namespace Mjolnir.CRM.SolutionManager.CLI.Commands.Solution
{
    [ConsoleCommandAttribute(
        Command = "ConvertPatchToSolution",
        Desription = "",
        DependentCommand = typeof(CrmConnectCommand))]
    public class ConvertPatchesToSolutionCommand : ConsoleCommandBase
    {
        [StringInput(Description = "Solution to be merged with patches.", IsRequired = true)]
        public string SelectedSolutionId { get; set; }

        public override object Execute(ITracingService tracer, object input)
        {
            CRMContext context = (CRMContext)input;

            var req = new ConvertPatchesToSolutionRequest()
            {
                SelectedSolutionIds = new string[] { SelectedSolutionId }
            };

            return new Mjolnir.CRM.SolutionManager.Business.SolutionBusiness().ConvertPatchToSolution(req, new ConvertPatchesToSolutionResponse(), context); ;

        }
    }
}
