using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mjolnir.CRM.JavaScriptOperation;
using Mjolnir.CRM.SolutionManager.Infrastructure.ConvertPatchesToSolution;
using Mjolnir.CRM.Common;

namespace Mjolnir.CRM.SolutionManager.JsOperations
{
    public class ConvertPatchesToSolutionOperation : JavaScriptOperationBase<ConvertPatchesToSolutionRequest, ConvertPatchesToSolutionResponse>
    {
        public override ConvertPatchesToSolutionResponse ExecuteInternal(ConvertPatchesToSolutionRequest req, ConvertPatchesToSolutionResponse res, CRMContext context)
        {
            return new Mjolnir.CRM.SolutionManager.Business.SolutionBusiness().ConvertPatchToSolution(req, res, context); ;
        }
    }
}