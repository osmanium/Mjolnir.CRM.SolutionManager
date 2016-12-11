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
        public string SolutionName { get; set; }
    }

    public class ConvertAllPatchesToSolutionsReponse
    {
        //TODO: Update here, dummy
    }

    public class ConvertAllPatchesToSolutionsOperation : JavaScriptOperationBase<ConvertAllPatchesToSolutionsRequest, ConvertAllPatchesToSolutionsReponse>
    {
        public override ConvertAllPatchesToSolutionsReponse ExecuteInternal(ConvertAllPatchesToSolutionsReponse req)
        {
            //TODO: Some operations

            return null;
        }
    }
}
