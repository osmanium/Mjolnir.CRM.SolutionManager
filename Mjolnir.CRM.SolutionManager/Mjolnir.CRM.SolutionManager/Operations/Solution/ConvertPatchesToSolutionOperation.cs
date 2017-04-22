using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mjolnir.CRM.JavaScriptOperation;
using Mjolnir.CRM.SolutionManager.Infrastructure.ConvertPatchesToSolution;
using Mjolnir.CRM.Core;

namespace Mjolnir.CRM.SolutionManager.Operations.Solution
{
    //public class ConvertPatchesToSolutionOperation : JavaScriptOperationBase<ConvertPatchesToSolutionRequest, ConvertPatchesToSolutionResponse>
    //{
    //    public override ConvertPatchesToSolutionResponse ExecuteInternal(ConvertPatchesToSolutionRequest req, ConvertPatchesToSolutionResponse res, CRMContext context)
    //    {
    //        return new Mjolnir.CRM.SolutionManager.Business.SolutionBusiness().ConvertPatchToSolution(req, res, context); ;
    //    }
    //}

    //[ConsoleCommandAttribute(
    //    Command = "ConvertPatchToSolution",
    //    Desription = "",
    //    DependentCommand = typeof(CrmConnectCommand))]
    //public class ConvertPatchesToSolutionCommand : ConsoleCommandBase
    //{
    //    [StringInput(Description = "Solution uniqye name to be merged with patches.", IsRequired = true)]
    //    public string SelectedSolutionUniqueName { get; set; }

    //    public override object Execute(ITracingService tracer, object input)
    //    {
    //        CrmContext context = (CrmContext)input;

    //        var solutionId = Core.EntityManagers.SolutionManager.GetSolutionIdByUniqueSolutionName(SelectedSolutionId);
    //        if (solutionId != Guid.Empty)
    //        {
    //            var req = new ConvertPatchesToSolutionRequest()
    //            {
    //                SelectedSolutionIds = new string[] { solutionId }
    //            };

    //            return new Mjolnir.CRM.SolutionManager.Business.SolutionBusiness().ConvertPatchToSolution(req, new ConvertPatchesToSolutionResponse(), context); ;
    //        }
    //        else
    //        {
    //            Console.WriteLine($"Solution with name : {SelectedSolutionUniqueName} not found.");
    //        }
    //    }
    //}
}