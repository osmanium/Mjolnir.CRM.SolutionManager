using Common;
using JavaScriptOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSolutionManager.JsOperations
{
    public class ApplySolutionUpgradeRequest : JavaScriptOperationRequestBase
    {
        //TODO: Update here, dummy
    }

    public class ApplySolutionUpgradeReponse : JavaScriptOperationResponseBase
    {
        //TODO: Update here, dummy
    }

    public class ApplySolutionUpgradeOperation : JavaScriptOperationBase<ApplySolutionUpgradeRequest, ApplySolutionUpgradeReponse>
    {
        public override ApplySolutionUpgradeReponse ExecuteInternal(ApplySolutionUpgradeRequest req, ApplySolutionUpgradeReponse res, PluginContext context)
        {
            //TODO: Some operations

            context.TracingService.Trace("from the ConvertPatchesToSolutionsOperation execute internal");
            return res;
        }
    }
}
