using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.EntityManagers
{
    public class SolutionManager : EntityManagerBase
    {
        public SolutionManager(CRMContext context)
            : base(context)
        { }


        public EntityCollection GetAllSolutions()
        {
            try
            {
                context.TracingService.Trace("GetAllSolutions started.");

                string[] retrieveSolutionColumns = new string[] {
                    EntityAttributes.SolutionEntityAttributes.FriendlyNameFieldName,
                    EntityAttributes.SolutionEntityAttributes.ParentSolutionIdFieldName,
                    EntityAttributes.SolutionEntityAttributes.UniqueNameFieldName,
                    EntityAttributes.SolutionEntityAttributes.VersionFieldName,
                    EntityAttributes.SolutionEntityAttributes.IsManagedFieldName,
                    EntityAttributes.SolutionEntityAttributes.Description
                };

                QueryExpression query = new QueryExpression(EntityAttributes.SolutionEntityAttributes.EntityName);
                query.ColumnSet = new ColumnSet(retrieveSolutionColumns);

                return context.OrganizationService.RetrieveMultiple(query);
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
                return null;
            }
        }

        public string GetUniqueSolutionName(Entity solution)
        {
            context.TracingService.Trace("GetUniqueSolutionName started.");

            try
            {
                if (solution.Contains(EntityAttributes.SolutionEntityAttributes.UniqueNameFieldName) &&
                        solution.GetAttributeValue<string>(EntityAttributes.SolutionEntityAttributes.UniqueNameFieldName) != null)
                {
                    return solution.GetAttributeValue<string>(EntityAttributes.SolutionEntityAttributes.UniqueNameFieldName);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return string.Empty;
        }

        public bool IsPatchSolution(Entity solution)
        {
            context.TracingService.Trace("IsPatchSolution started.");

            try
            {
                if (solution.Contains(EntityAttributes.SolutionEntityAttributes.ParentSolutionIdFieldName) &&
                        solution.GetAttributeValue<EntityReference>(EntityAttributes.SolutionEntityAttributes.ParentSolutionIdFieldName) != null)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return false;
        }

        public bool IsManagedSolution(Entity solution)
        {
            context.TracingService.Trace("IsManagedSolution started.");

            try
            {
                if (solution.Contains(EntityAttributes.SolutionEntityAttributes.IsManagedFieldName) &&
                        solution.GetAttributeValue<bool>(EntityAttributes.SolutionEntityAttributes.IsManagedFieldName) == true)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return false;
        }

        public bool UpdateSolutionVersion(Entity solution, Version newVersion)
        {
            context.TracingService.Trace("UpdateSolutionVersion started.");

            var result = false;

            try
            {
                if (solution.Contains(EntityAttributes.SolutionEntityAttributes.VersionFieldName))
                    solution[EntityAttributes.SolutionEntityAttributes.VersionFieldName] = newVersion.ToString();

                context.OrganizationService.Update(solution);
                result = true;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return result;
        }

        public bool IsPatchSolutionBySolutionId(Guid solutionId)
        {
            context.TracingService.Trace("IsPatchSolutionBySolutionId started.");

            var result = false;

            try
            {
                var solution = GetSolutionById(solutionId);
                result = IsPatchSolution(solution);

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result;
        }

        public EntityReference GetParentSolutionReference(Entity solution)
        {
            context.TracingService.Trace("GetParentSolutionReference started.");

            try
            {
                if (solution.Contains(EntityAttributes.SolutionEntityAttributes.ParentSolutionIdFieldName) &&
                        solution.GetAttributeValue<EntityReference>(EntityAttributes.SolutionEntityAttributes.ParentSolutionIdFieldName) != null)
                {
                    return solution.GetAttributeValue<EntityReference>(EntityAttributes.SolutionEntityAttributes.ParentSolutionIdFieldName);
                }
                else return null;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return null;
            }
        }

        public CloneAsSolutionResponse CloneAsSolution(Entity solution)
        {
            context.TracingService.Trace("CloneAsSolution started.");

            try
            {
                CloneAsSolutionRequest cloneAsSolutionRequest = new CloneAsSolutionRequest()
                {
                    ParentSolutionUniqueName = solution.GetAttributeValue<string>(EntityAttributes.SolutionEntityAttributes.UniqueNameFieldName),
                    DisplayName = solution.GetAttributeValue<string>(EntityAttributes.SolutionEntityAttributes.FriendlyNameFieldName),
                    VersionNumber = CalculateNewVersion(
                            new Version(solution.GetAttributeValue<string>(EntityAttributes.SolutionEntityAttributes.VersionFieldName))).ToString()
                };

                return (CloneAsSolutionResponse)this.context.OrganizationService.Execute(cloneAsSolutionRequest);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return null;
            }
        }

        public Entity GetSolutionById(Guid solutionId)
        {
            context.TracingService.Trace("GetSolutionById started.");

            try
            {
                return context.OrganizationService.Retrieve(EntityAttributes.SolutionEntityAttributes.EntityName, solutionId, new ColumnSet(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return null;
            }
        }

        public EntityCollection GetPatchesBySolutionId(Guid solutionId)
        {
            context.TracingService.Trace("GetPatchesBySolutionId started.");

            try
            {
                string[] retrieveSolutionColumns = new string[] {
                    EntityAttributes.SolutionEntityAttributes.FriendlyNameFieldName,
                    EntityAttributes.SolutionEntityAttributes.ParentSolutionIdFieldName,
                    EntityAttributes.SolutionEntityAttributes.UniqueNameFieldName,
                    EntityAttributes.SolutionEntityAttributes.VersionFieldName,
                    EntityAttributes.SolutionEntityAttributes.IsManagedFieldName
                };

                QueryExpression query = new QueryExpression(EntityAttributes.SolutionEntityAttributes.EntityName);
                query.ColumnSet = new ColumnSet(retrieveSolutionColumns);
                query.Criteria.AddCondition(EntityAttributes.SolutionEntityAttributes.ParentSolutionIdFieldName, ConditionOperator.Equal, solutionId);

                return context.OrganizationService.RetrieveMultiple(query);
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
                return null;
            }
        }

        public Entity CreateSolution(Guid publisherId, string friendlyName, string description, Version version)
        {
            context.TracingService.Trace("CreateSolution started.");

            try
            {
                //TODO : Validate

                var solution = new Entity(EntityAttributes.SolutionEntityAttributes.EntityName);

                solution.Attributes.Add(EntityAttributes.SolutionEntityAttributes.FriendlyNameFieldName, friendlyName);
                solution.Attributes.Add(EntityAttributes.SolutionEntityAttributes.UniqueNameFieldName, friendlyName);
                solution.Attributes.Add(EntityAttributes.SolutionEntityAttributes.PublisherId, new EntityReference(EntityAttributes.PublisherEntityAttributes.EntityName, publisherId));
                solution.Attributes.Add(EntityAttributes.SolutionEntityAttributes.Description, description);
                solution.Attributes.Add(EntityAttributes.SolutionEntityAttributes.VersionFieldName, version.ToString());


                var newSolutionId = this.context.OrganizationService.Create(solution);
                solution.Attributes[EntityAttributes.SolutionEntityAttributes.IdFieldName] = newSolutionId;

                return solution;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return null;
            }
        }

        public EntityCollection RetrieveSolutionComponents(Entity patch)
        {
            context.TracingService.Trace("RetrieveSolutionComponents started.");

            try
            {
                var patchSolutionId = patch.Id;

                string[] retrieveSolutionComponentColumns = new string[] {
                    EntityAttributes.SolutionComponentEntityAttributes.SolutionComponentId,
                    EntityAttributes.SolutionComponentEntityAttributes.ComponentType,
                    EntityAttributes.SolutionComponentEntityAttributes.SolutionId,
                    EntityAttributes.SolutionComponentEntityAttributes.ObjectId,
                    EntityAttributes.SolutionComponentEntityAttributes.IsMetadata,
                    EntityAttributes.SolutionComponentEntityAttributes.RootComponentBehavior,
                };


                QueryExpression query = new QueryExpression(EntityAttributes.SolutionComponentEntityAttributes.EntityName);
                query.ColumnSet = new ColumnSet(retrieveSolutionComponentColumns);
                query.Criteria.AddCondition(EntityAttributes.SolutionComponentEntityAttributes.SolutionId, ConditionOperator.Equal, new object[] { patchSolutionId });

                var solutionComponentsEntityCollection = context.OrganizationService.RetrieveMultiple(query);

                if (solutionComponentsEntityCollection != null && solutionComponentsEntityCollection.Entities.Any())
                {
                    return solutionComponentsEntityCollection;
                }

                return null;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return null;
            }
        }

        public void CopySolutionComponents(Entity newSolution, EntityCollection solutionComponentsEntityCollection)
        {
            context.TracingService.Trace("CopySolutionComponents started.");

            //TODO : May throw error for duplicate, check this scenario

            try
            {
                var solutionUniqueName = newSolution.GetAttributeValue<string>(EntityAttributes.SolutionEntityAttributes.UniqueNameFieldName);

                if (solutionComponentsEntityCollection != null && solutionComponentsEntityCollection.Entities.Any())
                {
                    foreach (var component in solutionComponentsEntityCollection.Entities)
                    {
                        var componentType = component.GetAttributeValue<OptionSetValue>(EntityAttributes.SolutionComponentEntityAttributes.ComponentType).Value;
                        var isMetadata = component.GetAttributeValue<bool>(EntityAttributes.SolutionComponentEntityAttributes.IsMetadata);

                        AddSolutionComponentRequest addSolutionComponentRequest = new AddSolutionComponentRequest()
                        {
                            AddRequiredComponents = false,
                            ComponentType = componentType,
                            SolutionUniqueName = solutionUniqueName,
                            ComponentId = component.GetAttributeValue<Guid>(EntityAttributes.SolutionComponentEntityAttributes.ObjectId),
                            RequestId = Guid.NewGuid()
                        };

                        if (component.Contains(EntityAttributes.SolutionComponentEntityAttributes.RootComponentBehavior))
                        {
                            var behaviour = (Constants.SolutionComponentRootComponentBehavior)component.GetAttributeValue<OptionSetValue>(EntityAttributes.SolutionComponentEntityAttributes.RootComponentBehavior).Value;
                            if (behaviour == Constants.SolutionComponentRootComponentBehavior.Donotincludesubcomponents && isMetadata == true)
                            {
                                addSolutionComponentRequest.DoNotIncludeSubcomponents = true;
                            }
                        }

                        var response = (AddSolutionComponentResponse)context.OrganizationService.Execute(addSolutionComponentRequest);
                    }
                }
                else
                {
                    context.TracingService.Trace("No component found for transferring.");
                }

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public bool IsPatchInDescription(Entity solution)
        {
            context.TracingService.Trace("IsPatchInDescription started.");

            try
            {
                if (solution.Contains(EntityAttributes.SolutionEntityAttributes.Description))
                {
                    var description = solution.GetAttributeValue<string>(EntityAttributes.SolutionEntityAttributes.Description);

                    //TODO: contastant
                    if (!string.IsNullOrWhiteSpace(description) && description.Contains("is_patch:true"))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return false;
        }

        public bool IsUpgradeForSolution(string parentSolutionUniqueName, Entity solution)
        {
            context.TracingService.Trace("IsUpgradeForSolution started.");

            try
            {
                if (solution.Contains(EntityAttributes.SolutionEntityAttributes.Description))
                {
                    var solutionName = GetUniqueSolutionName(solution);
                    var description = solution.GetAttributeValue<string>(EntityAttributes.SolutionEntityAttributes.Description);

                    //TODO: contastant
                    if (!string.IsNullOrWhiteSpace(description) && description.Contains("base_solution_uniquename:" + parentSolutionUniqueName))
                    {
                        return true;
                    }
                    else
                        context.TracingService.Trace("Solution Name : " + parentSolutionUniqueName);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return false;
        }

        public Version GetSolutionVersion(Entity solution)
        {
            return new Version(solution.GetAttributeValue<string>(EntityAttributes.SolutionEntityAttributes.VersionFieldName));
        }

        public void RemoveSolutions(List<Guid> solutionIds)
        {
            context.TracingService.Trace("RemoveSolutions started.");

            foreach (var solutionId in solutionIds)
            {
                try
                {
                    context.OrganizationService.Delete(EntityAttributes.SolutionEntityAttributes.EntityName, solutionId);
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        public EntityCollection GetSolutionUpgrades(Entity parentSolution)
        {
            context.TracingService.Trace("GetSolutionUpgrades started.");

            var upgradeSolutions = new EntityCollection();
            var sortedUpgradeSolutions = new EntityCollection();

            var allSolutions = GetAllSolutions();
            var parentSolutionName = GetUniqueSolutionName(parentSolution);

            foreach (var solution in allSolutions.Entities)
            {
                //context.TracingService.Trace("\n====\n" + solution.GetAttributeValue<string>(EntityAttributes.SolutionEntityAttributes.Description) + "\n====\n");

                if (IsPatchInDescription(solution) && IsUpgradeForSolution(parentSolutionName, solution))
                {
                    context.TracingService.Trace("Upgrade Solution : " + GetUniqueSolutionName(solution));
                    upgradeSolutions.Entities.Add(solution);
                }
            }

            //Send in ascending order in solution version
            if (upgradeSolutions.Entities.Any())
            {
                sortedUpgradeSolutions = new EntityCollection(upgradeSolutions.Entities.OrderByDescending(key => GetPatchVersionInDescription(key)).ToList());
            }

            return sortedUpgradeSolutions;
        }

        public Guid CreatePatchForBaseSolution(Entity parentSolution, Entity upgradeSolution)
        {
            context.TracingService.Trace("CreatePatchForBaseSolution started.");

            //Get the version of patch and create a real patch with the same version
            Version patchVersion = GetSolutionVersion(upgradeSolution);

            try
            {
                CloneAsPatchRequest patchRequest = new CloneAsPatchRequest();
                patchRequest.ParentSolutionUniqueName = GetUniqueSolutionName(parentSolution);
                patchRequest.VersionNumber = patchVersion.ToString();
                patchRequest.DisplayName = GetUniqueSolutionName(upgradeSolution);

                var response = (CloneAsPatchResponse)context.OrganizationService.Execute(patchRequest);

                return response.SolutionId;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return Guid.Empty;
            }
        }

        private Version GetPatchVersionInDescription(Entity upgradeSolution)
        {
            context.TracingService.Trace("GetPatchVersionInDescription started.");

            try
            {
                if (upgradeSolution.Contains(EntityAttributes.SolutionEntityAttributes.Description))
                {
                    var description = upgradeSolution.GetAttributeValue<string>(EntityAttributes.SolutionEntityAttributes.Description);

                    //TODO: contastant
                    if (description.Contains("is_patch:true"))
                    {
                        var lines = description.Split('\n');
                        var versionLine = lines.Where(w => w.Contains("base_solution_verison:")).FirstOrDefault();

                        if (!string.IsNullOrWhiteSpace(versionLine))
                        {
                            string version = versionLine.Substring(("base_solution_verison:").Length).Trim();

                            context.TracingService.Trace("version : " + version);

                            return new Version(version);
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return null;
            }
        }


        public Version CalculateNewVersion(Version oldVersion)
        {
            context.TracingService.Trace("CalculateNewVersion started.");
            return new Version(oldVersion.Major, oldVersion.Minor + 1, 0, 0);
        }
    }
}

