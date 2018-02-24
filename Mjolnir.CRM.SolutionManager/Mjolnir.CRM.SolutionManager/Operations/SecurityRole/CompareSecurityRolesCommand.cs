using Mjolnir.ConsoleCommandLine;
using Mjolnir.CRM.SolutionManager.Operations.CRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using Mjolnir.ConsoleCommandLine.Tracer;
using Mjolnir.CRM.Core;
using Mjolnir.CRM.Core.EntityManagers;
using Mjolnir.CRM.Sdk.Entities;
using Mjolnir.CRM.Sdk.Extensions;

namespace Mjolnir.CRM.SolutionManager.Operations.SecurityRole
{
    [Verb("Compare-SecurityRoles")]
    public class CompareSecurityRolesCommand : ConsoleCommandBase
    {
        
        [Option("firstrole",
            Required = true,
            HelpText = "Security Role to be compared")]
        public string SourceSecurityRoleName { get; set; }


        [Option("secondrole",
            Required = true,
            HelpText = "Security Role to be compared with")]
        public string TargetSecurityRoleName { get; set; }


        public override async Task<object> ExecuteCommand(ITracingService tracer, object input)
        {
            try
            {
                var sourceAndTargetCrmContextCommand = new ConnectCrmSourceAndTargetCommand();
                input = await sourceAndTargetCrmContextCommand.ExecuteCommand(tracer, input);
                

                var crmContexts = input as CrmContext[];

                var sourceCrmContext = crmContexts[0];
                var targetCrmContext = crmContexts[1];

                var differentSecurityRoles = new List<string>();

                if (sourceCrmContext != null)
                {
                    RoleManager sourceRoleManager = new RoleManager(sourceCrmContext);
                    RolePrivilegesManager sourceRolePrivilegesManager = new RolePrivilegesManager(sourceCrmContext);

                    RoleManager targetRoleManager = new RoleManager(targetCrmContext);
                    RolePrivilegesManager targetRolePrivilegesManager = new RolePrivilegesManager(targetCrmContext);

                    tracer.Trace("Retrieving all source security roles..");
                    var allSourceRoles = await sourceRoleManager.GetAllRootLevelRolesAsync();

                    tracer.Trace("Retrieving all target security roles..");
                    var allTargetRoles = await targetRoleManager.GetAllRootLevelRolesAsync();

                    tracer.Trace($"Source Role Count : {allSourceRoles.Count}, Target Role Count : {allTargetRoles.Count}");

                    tracer.Trace("Iterating source security roles..");
                    foreach (var sourceRole in allSourceRoles)
                    {
                        //Get the same security role from other organization
                        var targetRole = GetTargetRoleFromList(allTargetRoles, sourceRole.Id);

                        //Get both roles privileges
                        tracer.Trace($"Retrieving privileges for (Source) - {sourceRole.Name}..");
                        var sourceRolePrivileges = await sourceRolePrivilegesManager.GetRolePrivilegesAsync(sourceRole.Id);

                        tracer.Trace($"Retrieving privileges for (Target) - {targetRole.Name}..");
                        var targetRolePrivileges = await targetRolePrivilegesManager.GetRolePrivilegesAsync(targetRole.Id);

                        var sourceRoleStatus = !(sourceRolePrivileges == null || !sourceRolePrivileges.Any());
                        var targetRoleStatus = !(targetRolePrivileges == null || !targetRolePrivileges.Any());

                        //Both are null - OK/Empty
                        if (sourceRoleStatus == false && targetRoleStatus == false)
                        {
                            tracer.Trace($"Both roles are empty..");
                            break;
                        }

                        //Source does not have privilege but target has - Not OK
                        if (sourceRoleStatus == false && targetRoleStatus == true)
                        {
                            tracer.Trace($"Role : {sourceRole.Name} does not have privilege in source, but has in target..");
                            differentSecurityRoles.Add(sourceRole.Name);
                            break;
                        }

                        //Target does not have privilege but source has - Not OK
                        if (sourceRoleStatus == true && targetRoleStatus == false)
                        {
                            tracer.Trace($"Role : {targetRole.Name} does not have privilege in target, but has in source..");
                            differentSecurityRoles.Add(sourceRole.Name);
                            break;
                        }

                        //Counts are differnet - Not OK
                        if (sourceRolePrivileges.Count != targetRolePrivileges.Count)
                        {
                            tracer.Trace($"Role : {targetRole.Name} does not have same count of privileges in target and source..");
                            differentSecurityRoles.Add(sourceRole.Name);
                            break;
                        }

                        //Compare sourceRole - targetRole

                        var attributeCompareResult = true;

                        tracer.Trace($"Iterating source role {sourceRole.Name} privileges..");
                        foreach (var sourceRolePrivilege in sourceRolePrivileges)
                        {
                            var targetRolePrivilege = targetRolePrivileges.FirstOrDefault(w => w.Id == sourceRolePrivilege.Id);

                            if (targetRolePrivilege == null)
                            {
                                attributeCompareResult = false;
                                differentSecurityRoles.Add(sourceRole.Name);
                                break;
                            }

                            if (!sourceRolePrivilege.CompareValues(targetRolePrivilege).IsEqual)
                            {
                                attributeCompareResult = false;
                                differentSecurityRoles.Add(sourceRole.Name);
                                break;
                            }
                        }

                        if (!attributeCompareResult)
                        {
                            differentSecurityRoles.Add(sourceRole.Name);
                            break;
                        }

                        tracer.Trace($"Iterating target role {sourceRole.Name} privileges..");
                        foreach (var targetRolePrivilege in targetRolePrivileges)
                        {
                            var sourceRolePrivilege = sourceRolePrivileges.FirstOrDefault(w => w.Id == targetRolePrivilege.Id);

                            if (sourceRolePrivilege != null)
                            {
                                attributeCompareResult = false;
                                differentSecurityRoles.Add(sourceRole.Name);
                                break;
                            }
                        }

                        tracer.Trace($"{sourceRole.Name} role privileges are same..");
                    }
                }

                if (differentSecurityRoles.Any())
                {
                    differentSecurityRoles.ForEach((x) =>
                    {
                        tracer.Trace($"Role {x} is different..");
                    });
                }

                return null;

            }
            catch (Exception ex)
            {
                HandleCommandException(tracer, ex);
                return null;
            }
        }


        private RoleEntity GetTargetRoleFromList(List<RoleEntity> targetRoleList, Guid targetRoleId)
        {
            return targetRoleList.FirstOrDefault(w => w.Id == targetRoleId);
        }
    }
}
