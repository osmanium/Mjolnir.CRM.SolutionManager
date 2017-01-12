using Microsoft.Xrm.Sdk;
using Common;
using CRMSolutionManager.JsOperations;
using JavaScriptOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSolutionManager.JsOperations
{
    public class ConvertPatchesToSolutionsRequest : JavaScriptOperationRequestBase
    {
        //TODO: Update here, dummy
    }

    public class ConvertPatchesToSolutionsReponse : JavaScriptOperationResponseBase
    {
        //TODO: Update here, dummy
    }

    public class ConvertPatchesToSolutionsOperation : JavaScriptOperationBase<ConvertPatchesToSolutionsRequest, ConvertPatchesToSolutionsReponse>
    {
        public override ConvertPatchesToSolutionsReponse ExecuteInternal(ConvertPatchesToSolutionsRequest req, ConvertPatchesToSolutionsReponse res,PluginContext context)
        {
            //TODO: Some operations

            context.TracingService.Trace("from the ConvertPatchesToSolutionsOperation execute internal");
            return res;
        }
    }
}
