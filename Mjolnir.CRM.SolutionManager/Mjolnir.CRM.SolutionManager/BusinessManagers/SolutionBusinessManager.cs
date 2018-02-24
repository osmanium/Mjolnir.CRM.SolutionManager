using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using Mjolnir.CRM.Core;
using Mjolnir.CRM.Sdk.Entities;
using Mjolnir.CRM.SolutionManager.Infrastructure.ApplySolutionUpgrade;
using Mjolnir.CRM.SolutionManager.Infrastructure.ConvertPatchesToSolution;

namespace Mjolnir.CRM.SolutionManager.BusinessManagers
{
    public class SolutionBusiness
    {
        public ApplySolutionUpgradeResponse ApplySolutionUpgrade(ApplySolutionUpgradeRequest req, 
                                                                 ApplySolutionUpgradeResponse res, 
                                                                 CrmContext context)
        {
            #region Validations
            if (req.SelectedSolutionIds == null ||
                !req.SelectedSolutionIds.Any() ||
                string.IsNullOrWhiteSpace(req.SelectedSolutionIds.FirstOrDefault()))
                throw new ArgumentNullException(nameof(req.SelectedSolutionIds), "SelectedSolutionId cannot be null");
            #endregion


            var solutionManager = new Core.EntityManagers.SolutionManager(context);

            //Selected solution id
            var solutionId = new Guid(req.SelectedSolutionIds.FirstOrDefault());
            var selectedSolution = solutionManager.RetrieveById(solutionId);

            //If not patch solution and not a patch in description, then continue
            if (!solutionManager.IsPatchSolution(selectedSolution) && !solutionManager.IsPatchInDescription(selectedSolution))
            {
                //Brings the solutions which has patch information in description for given parent solution,
                //so that real patch will be created for real solution,
                //solutions should be in ascending order, otherwise loop will fail due to lower version
                var solutionUpgrades = solutionManager.GetSolutionUpgrades(selectedSolution);

                if (solutionUpgrades != null && solutionUpgrades.Entities.Any())
                {
                    context.TracingService.TraceInfo("solution upgrades count : " + solutionUpgrades.Entities.Count);

                    List<Guid> upgradeSolutionIds = new List<Guid>();

                    foreach (var solutionUpgrade in solutionUpgrades.Entities)
                    {
                        var solutionComponentsEntityCollection = solutionManager.RetrieveSolutionComponents(solutionUpgrade);

                        solutionManager.CopySolutionComponents(selectedSolution, new EntityCollection(solutionComponentsEntityCollection.Select(s => (Entity)s).ToList()));

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

        public ConvertPatchesToSolutionResponse ConvertPatchToSolution(ConvertPatchesToSolutionRequest req, 
                                                                       ConvertPatchesToSolutionResponse res, 
                                                                       CrmContext context)
        {
            #region Validations
            if (req.SelectedSolutionIds == null ||
                !req.SelectedSolutionIds.Any() ||
                string.IsNullOrWhiteSpace(req.SelectedSolutionIds.FirstOrDefault()))
                throw new ArgumentNullException("SelectedSolutionId", "SelectedSolutionId cannot be null");
            #endregion

            //Selected solution id
            var solutionId = new Guid(req.SelectedSolutionIds.FirstOrDefault());

            var solutionManager = new Mjolnir.CRM.Core.EntityManagers.SolutionManager(context);

            //Validate whether selected solution is not patch
            var isPatch = solutionManager.IsPatchSolutionBySolutionId(solutionId);
            if (isPatch)
                throw new InvalidCastException("Please select a parent solution, not a patch solution.");

            //Get patches
            var solutionPatches = solutionManager.GetPatchesBySolutionId(solutionId);
            if (solutionPatches == null || solutionPatches.Any() == false)
                throw new Exception("No patch found to convert.");
            context.TracingService.TraceInfo("Patches are fetched, count : " + solutionPatches.Count);

            var selectedSolution = solutionManager.RetrieveById(solutionId);

            var now = DateTime.Now;

            var solutionUniqueName = selectedSolution.GetAttributeValue<string>(EntityAttributes.SolutionEntityAttributes.UniqueNameFieldName);
            var solutionFriendlyName = selectedSolution.GetAttributeValue<string>(EntityAttributes.SolutionEntityAttributes.FriendlyNameFieldName);
            var solutionVersion = selectedSolution.GetAttributeValue<string>(EntityAttributes.SolutionEntityAttributes.VersionFieldName);
            var solutionPublisher = selectedSolution.GetAttributeValue<EntityReference>(EntityAttributes.SolutionEntityAttributes.PublisherId);

            //Generate solution name
            var newSolutionName = $"{solutionUniqueName}_{now.Year}_{now.Month.ToString().PadLeft(2, '0')}_{now.Day.ToString().PadLeft(2, '0')}_T{now.Hour.ToString().PadLeft(2, '0')}_{now.Minute.ToString().PadLeft(2, '0')}";
            var newDescription = $"is_patch:true\nbase_solution_verison:{solutionVersion}\nbase_solution_uniquename:{solutionUniqueName}";
            var newVersion = solutionPatches
                .Select(s => new Version(s.GetAttributeValue<string>(EntityAttributes.SolutionEntityAttributes.VersionFieldName)))
                .OrderByDescending(o => o)
                .FirstOrDefault();


            //Create Solution
            context.TracingService.TraceInfo($"New solution is being created : {newSolutionName}");
            var newSolutionEntity = solutionManager.CreateSolution(solutionPublisher.Id, newSolutionName, newDescription, solutionManager.CalculateNewVersion(newVersion));

            foreach (var solutionPatch in solutionPatches)
            {
                var patchUniqueName = solutionPatch.GetAttributeValue<string>(EntityAttributes.SolutionEntityAttributes.UniqueNameFieldName);

                //Include Components
                context.TracingService.TraceInfo($"Fetching components for patch : {patchUniqueName}");
                var solutionComponentsEntityCollection = solutionManager.RetrieveSolutionComponents(solutionPatch);

                context.TracingService.TraceInfo($"Including components from patch : {patchUniqueName}, to : {newSolutionName}");
                solutionManager.CopySolutionComponents(newSolutionEntity, new EntityCollection(solutionComponentsEntityCollection.Select(s => (Entity)s).ToList()));
            }

            context.TracingService.TraceInfo($"Cloning as solution : {newSolutionName}");
            solutionManager.CloneAsSolution(selectedSolution);

            context.TracingService.TraceInfo("End of function ConvertAllPatchesToSolution()");

            res.NewSolutionName = newSolutionName;

            return res;
        }

        //public PublishAllResponse PublishAll(PublishAllRequest req, PublishAllResponse res, CrmContext context)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
