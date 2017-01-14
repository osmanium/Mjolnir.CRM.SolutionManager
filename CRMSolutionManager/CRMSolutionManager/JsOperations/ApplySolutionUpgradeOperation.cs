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
        public string[] SelectedSolutionIds { get; set; }
    }

    public class ApplySolutionUpgradeReponse : JavaScriptOperationResponseBase
    {
    }

    public class ApplySolutionUpgradeOperation : JavaScriptOperationBase<ApplySolutionUpgradeRequest, ApplySolutionUpgradeReponse>
    {
        public override ApplySolutionUpgradeReponse ExecuteInternal(ApplySolutionUpgradeRequest req, ApplySolutionUpgradeReponse res, CRMContext context)
        {
            #region Validations
            if (req.SelectedSolutionIds == null ||
                !req.SelectedSolutionIds.Any() ||
                string.IsNullOrWhiteSpace(req.SelectedSolutionIds.FirstOrDefault()))
                throw new ArgumentNullException("SelectedSolutionId", "SelectedSolutionId cannot be null");
            #endregion


            var solutionManager = new SolutionManager(context);

            //Selected solution id
            var solutionId = new Guid(req.SelectedSolutionIds.FirstOrDefault());
            var selectedSolution = solutionManager.GetSolutionById(solutionId);

            //If not patch solution and not a patch in description, then continue
            if (!solutionManager.IsPatchSolution(selectedSolution) && !solutionManager.IsPatchInDescription(selectedSolution))
            {
                //Brings the solutions which has patch information in description for given parent solution,
                //so that real patch will be created for real solution,
                //solutions should be in ascending order, otherwise loop will fail due to lower version
                var solutionUpgrades = solutionManager.GetSolutionUpgrades(selectedSolution);

                if (solutionUpgrades != null && solutionUpgrades.Entities.Any())
                {
                    context.TracingService.Trace("solution upgrades count : " + solutionUpgrades.Entities.Count);

                    List<Guid> upgradeSolutionIds = new List<Guid>();

                    foreach (var solutionUpgrade in solutionUpgrades.Entities)
                    {
                        var solutionComponentsEntityCollection = solutionManager.RetrieveSolutionComponents(solutionUpgrade);

                        solutionManager.CopySolutionComponents(selectedSolution, solutionComponentsEntityCollection);

                        upgradeSolutionIds.Add(solutionUpgrade.Id);
                    }

                    var newVersion = solutionUpgrades.Entities
                                    .Select(s => new Version(s.GetAttributeValue<string>(EntityAttributes.SolutionEntityAttributes.VersionFieldName)))
                                    .OrderByDescending(o => o)
                                    .FirstOrDefault();

                    var updateVersionResponse = solutionManager.UpdateSolutionVersion(selectedSolution, newVersion);

                    if (updateVersionResponse)
                        solutionManager.RemoveSolutions(upgradeSolutionIds);
                }
                else
                    throw new Exception("No upgrade found.");
            }
            else
                throw new Exception("Please select a parent solution.");

            return res;
        }
    }
}
