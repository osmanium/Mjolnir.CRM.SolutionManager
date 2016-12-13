using Microsoft.Xrm.Sdk;
using OG.CRM.Common;
using OG.CRM.JavaScriptOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OG.CRM.CRMSolutionManager.JsOperations
{
    public class ConvertAllPatchesToSolutionsRequest : JavaScriptOperationRequestBase
    {
        //TODO: Update here, dummy
    }

    public class ConvertAllPatchesToSolutionsReponse : JavaScriptOperationResponseBase
    {
        //TODO: Update here, dummy
    }

    public class ConvertAllPatchesToSolutionsOperation : JavaScriptOperationBase<ConvertAllPatchesToSolutionsRequest, ConvertAllPatchesToSolutionsReponse>
    {
        public override ConvertAllPatchesToSolutionsReponse ExecuteInternal(ConvertAllPatchesToSolutionsRequest req, PluginContext context)
        {
            //TODO: Some operations

            context.TracingService.Trace("from the ConvertAllPatchesToSolutionsOperation execute internal");
            return new ConvertAllPatchesToSolutionsReponse();
        }
    }
}
