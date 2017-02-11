using Mjolnir.CRM.Common;
using Mjolnir.CRM.Common.EntityManagers;
using Mjolnir.CRM.JavaScriptOperation;
using Mjolnir.CRM.SolutionManager.Business;
using Mjolnir.CRM.SolutionManager.Infrastructure;
using Mjolnir.CRM.SolutionManager.Infrastructure.ApplySolutionUpgrade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mjolnir.CRM.SolutionManager.JsOperations
{
    public class ApplySolutionUpgradeOperation : JavaScriptOperationBase<ApplySolutionUpgradeRequest, ApplySolutionUpgradeResponse>
    {
        public override ApplySolutionUpgradeResponse ExecuteInternal(ApplySolutionUpgradeRequest req, ApplySolutionUpgradeResponse res, CRMContext context)
        {
            return new SolutionBusiness().ApplySolutionUpgrade(req, res, context); ;
        }
    }
}
