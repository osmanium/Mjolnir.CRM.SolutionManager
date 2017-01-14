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
    public class ConvertPatchesToSolutionRequest : JavaScriptOperationRequestBase
    {
        public string[] SelectedSolutionIds { get; set; }
    }

    public class ConvertPatchesToSolutionReponse : JavaScriptOperationResponseBase
    {
        public string NewSolutionName { get; set; }
    }

    public class ConvertPatchesToSolutionsOperation : JavaScriptOperationBase<ConvertPatchesToSolutionRequest, ConvertPatchesToSolutionReponse>
    {
        public override ConvertPatchesToSolutionReponse ExecuteInternal(ConvertPatchesToSolutionRequest req, ConvertPatchesToSolutionReponse res, CRMContext context)
        {
            #region Validations
            if (req.SelectedSolutionIds == null ||
                !req.SelectedSolutionIds.Any() ||
                string.IsNullOrWhiteSpace(req.SelectedSolutionIds.FirstOrDefault()))
                throw new ArgumentNullException("SelectedSolutionId", "SelectedSolutionId cannot be null");
            #endregion

            //Selected solution id
            var solutionId = new Guid(req.SelectedSolutionIds.FirstOrDefault());

            var solutionManager = new SolutionManager(context);

            //Validate whether selected solution is not patch
            var isPatch = solutionManager.IsPatchSolutionBySolutionId(solutionId);
            if (isPatch)
                throw new InvalidCastException("Please select a parent solution, not a patch solution.");

            //Get patches
            var solutionPatches = solutionManager.GetPatchesBySolutionId(solutionId);
            if (solutionPatches == null || solutionPatches.Entities.Any() == false)
                throw new Exception("No patch found to convert.");
            context.TracingService.Trace("Patches are fetched, count : " + solutionPatches.Entities.Count);

            var selectedSolution = solutionManager.GetSolutionById(solutionId);

            var now = DateTime.Now;

            var solutionUniqueName = selectedSolution.GetAttributeValue<string>(EntityAttributes.SolutionEntityAttributes.UniqueNameFieldName);
            var solutionFriendlyName = selectedSolution.GetAttributeValue<string>(EntityAttributes.SolutionEntityAttributes.FriendlyNameFieldName);
            var solutionVersion = selectedSolution.GetAttributeValue<string>(EntityAttributes.SolutionEntityAttributes.VersionFieldName);
            var solutionPublisher = selectedSolution.GetAttributeValue<EntityReference>(EntityAttributes.SolutionEntityAttributes.PublisherId);

            //Generate solution name
            var newSolutionName = $"{solutionUniqueName}_{now.Year}_{now.Month.ToString().PadLeft(2, '0')}_{now.Day.ToString().PadLeft(2, '0')}_T{now.Hour.ToString().PadLeft(2, '0')}_{now.Minute.ToString().PadLeft(2, '0')}";
            var newDescription = $"is_patch:true\nbase_solution_verison:{solutionVersion}\nbase_solution_uniquename:{solutionUniqueName}";
            var newVersion = solutionPatches.Entities
                .Select(s => new Version(s.GetAttributeValue<string>(EntityAttributes.SolutionEntityAttributes.VersionFieldName)))
                .OrderByDescending(o => o)
                .FirstOrDefault();


            //Create Solution
            context.TracingService.Trace($"New solution is being created : {newSolutionName}");
            var newSolutionEntity = solutionManager.CreateSolution(solutionPublisher.Id, newSolutionName, newDescription, solutionManager.CalculateNewVersion(newVersion));

            foreach (var solutionPatch in solutionPatches.Entities)
            {
                var patchUniqueName = solutionPatch.GetAttributeValue<string>(EntityAttributes.SolutionEntityAttributes.UniqueNameFieldName);

                //Include Components
                context.TracingService.Trace($"Fetching components for patch : {patchUniqueName}");
                var solutionComponentsEntityCollection = solutionManager.RetrieveSolutionComponents(solutionPatch);

                context.TracingService.Trace($"Including components from patch : {patchUniqueName}, to : {newSolutionName}");
                solutionManager.CopySolutionComponents(newSolutionEntity, solutionComponentsEntityCollection);
            }

            context.TracingService.Trace($"Cloning as solution : {newSolutionName}");
            solutionManager.CloneAsSolution(selectedSolution);

            context.TracingService.Trace("End of function ConvertAllPatchesToSolution()");
            
            res.NewSolutionName = newSolutionName;

            return res;
        }
    }
}