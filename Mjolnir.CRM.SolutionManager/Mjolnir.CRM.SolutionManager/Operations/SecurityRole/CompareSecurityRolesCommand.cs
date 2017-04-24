using Mjolnir.ConsoleCommandLine;
using Mjolnir.CRM.SolutionManager.Operations.CRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Mjolnir.ConsoleCommandLine.InputAttributes;
using Mjolnir.CRM.Core;
using Mjolnir.CRM.SolutionManager.BusinessManagers;
using Mjolnir.CRM.Core.EntityManagers;
using Microsoft.Xrm.Sdk.Client;
using System.Configuration;
using Microsoft.Xrm.Tooling.Connector;
using System.Net;
using Mjolnir.CRM.Sdk.Extensions;
using Mjolnir.CRM.Sdk.Entities;

namespace Mjolnir.CRM.SolutionManager.Operations.SecurityRole
{
    [ConsoleCommandAttribute(
        Command = "CompareSecurityRoles",
        Desription = "",
        DependentCommand = typeof(ConnectCrmSourceAndTargetCommand))]
    public class CompareSecurityRolesCommand : ConsoleCommandBase
    {
        [StringInput(Description = "Security Role to be compared", IsRequired = true)]
        public string SourceSecurityRoleName { get; set; }


        [StringInput(Description = "Security Role to be compared with", IsRequired = true)]
        public string TargetSecurityRoleName { get; set; }

        public override object ExecuteCommand(ITracingService tracer, object input)
        {
            try
            {


                if (input == null)
                {
                    tracer.Trace($"Connections are not avilable, {typeof(CompareSecurityRolesCommand).Name} is cancelled..");
                    return null;
                }

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
                    var allSourceRoles = sourceRoleManager.GetAllRootLevelRoles();

                    tracer.Trace("Retrieving all target security roles..");
                    var allTargetRoles = targetRoleManager.GetAllRootLevelRoles();

                    tracer.Trace($"Source Role Count : {allSourceRoles.Count}, Target Role Count : {allTargetRoles.Count}");

                    tracer.Trace("Iterating source security roles..");
                    foreach (var sourceRole in allSourceRoles)
                    {
                        //Get the same security role from other organization
                        var targetRole = GetTargetRoleFromList(allTargetRoles, sourceRole.Id);

                        //Get both roles privileges
                        tracer.Trace($"Retrieving privileges for (Source) - {sourceRole.Name}..");
                        var sourceRolePrivileges = sourceRolePrivilegesManager.GetRolePrivileges(sourceRole.Id);

                        tracer.Trace($"Retrieving privileges for (Target) - {targetRole.Name}..");
                        var targetRolePrivileges = targetRolePrivilegesManager.GetRolePrivileges(targetRole.Id);

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
                            var targetRolePrivilege = targetRolePrivileges.Where(w => w.Id == sourceRolePrivilege.Id).FirstOrDefault();

                            if (targetRolePrivilege == null)
                            {
                                attributeCompareResult = false;
                                differentSecurityRoles.Add(sourceRole.Name);
                                break;
                            }

                            if (!sourceRolePrivilege.Compare(targetRolePrivilege))
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
                            var sourceRolePrivilege = sourceRolePrivileges.Where(w => w.Id == targetRolePrivilege.Id).FirstOrDefault();

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
            return targetRoleList.Where(w => w.Id == targetRoleId).FirstOrDefault();
        }
    }
}
