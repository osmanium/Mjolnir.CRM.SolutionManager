using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using Mjolnir.ConsoleCommandLine;
using Mjolnir.CRM.Core;
using Mjolnir.CRM.Core.Loggers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mjolnir.CRM.SolutionManager.Operations.CRM
{
    [ConsoleCommandAttribute(
        Command = "ConnectCrmTarget",
        Desription = "")]
    public class ConnectCrmTargetCommand : ConsoleCommandBase
    {
        private static OrganizationServiceProxy ConnectCrm(string userName, string serverName, string password, string domain, string port, string organization, bool useSSL)
        {
            CrmServiceClient _sourceCRMServiceClient = new CrmServiceClient(new NetworkCredential(userName, password, domain), serverName, port, organization, true, useSSL, null);
            return _sourceCRMServiceClient.OrganizationServiceProxy;
        }

        public override object ExecuteCommand(ITracingService tracer, object input)
        {
            OrganizationServiceProxy _orgService = null;

            tracer.Trace($"Reading Target CRM connection configuration...");

            var targetUserName = ConfigurationManager.AppSettings["TargetUsername"];
            tracer.Trace($"TargetUsername = {targetUserName}...");

            var targetServer = ConfigurationManager.AppSettings["TargetServer"];
            tracer.Trace($"TargetServer = {targetServer}...");

            var targetDomain = ConfigurationManager.AppSettings["TargetDomain"];
            tracer.Trace($"TargetDomain = {targetDomain}...");

            var targetPassword = ConfigurationManager.AppSettings["TargetPassword"];
            tracer.Trace($"TargetPassword = {targetPassword}...");

            var targetPort = ConfigurationManager.AppSettings["TargetPort"];
            tracer.Trace($"TargetPort = {targetPort}...");

            var targetOrganizationName = ConfigurationManager.AppSettings["TargetOrganizationName"];
            tracer.Trace($"TargetOrganizationName = {targetOrganizationName}...");

            var targetUseSSL = Boolean.Parse(ConfigurationManager.AppSettings["TargetUseSSL"]);
            tracer.Trace($"TargetUseSSL = {targetUseSSL}...");


            try
            {
                tracer.Trace($"Connecting CRM...");

                _orgService = ConnectCrm(targetUserName, targetServer, targetPassword, targetDomain, targetPort, targetOrganizationName, targetUseSSL);
                if (_orgService == null)
                {
                    tracer.Trace($"Target Connection was not successful...");
                    return null;
                }
                else
                {
                    tracer.Trace($"Target Connection successful...");

                    int timeOutInMinutes;
                    if (int.TryParse(ConfigurationManager.AppSettings[Constants.TimeOutInMinutes], out timeOutInMinutes))
                    {
                        _orgService.Timeout = new TimeSpan(0, timeOutInMinutes, 0);
                    }
                    else
                    {
                        _orgService.Timeout = new TimeSpan(0, 30, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleCommandException(tracer, ex);
                return null;
            }
            
            return new CrmContext(_orgService, _orgService.CallerId, new CrmLogger(tracer, System.Diagnostics.TraceLevel.Info), null, null);
        }
    }
}
