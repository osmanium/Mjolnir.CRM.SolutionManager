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

namespace Mjolnir.CRM.SolutionManager.CLI.Commands
{
    [ConsoleCommandAttribute(
        Command = "CrmConnect",
        Desription = "")]
    public class CrmConnectCommand : ConsoleCommandBase
    {
        private static OrganizationServiceProxy ConnectCrm(string userName, string serverName, string password, string domain, string port, string organization, bool useSSL)
        {
            CrmServiceClient _sourceCRMServiceClient = new CrmServiceClient(new NetworkCredential(userName, password, domain), serverName, port, organization, true, useSSL, null);
            return _sourceCRMServiceClient.OrganizationServiceProxy;
        }

        public override object Execute(ITracingService tracer, object input)
        {
            OrganizationServiceProxy _orgService = null;

            tracer.Trace($"Reading CRM connection configuration...");

            var sourceUserName = ConfigurationManager.AppSettings["Username"];
            tracer.Trace($"Username = {sourceUserName}...");

            var sourceServer = ConfigurationManager.AppSettings["Server"];
            tracer.Trace($"Server = {sourceServer}...");

            var sourceDomain = ConfigurationManager.AppSettings["Domain"];
            tracer.Trace($"Domain = {sourceDomain}...");

            var sourcePassword = ConfigurationManager.AppSettings["Password"];
            tracer.Trace($"Password = {sourcePassword}...");

            var sourcePort = ConfigurationManager.AppSettings["Port"];
            tracer.Trace($"Port = {sourcePort}...");

            var sourceOrganizationName = ConfigurationManager.AppSettings["OrganizationName"];
            tracer.Trace($"OrganizationName = {sourceOrganizationName}...");

            var sourceUseSSL = Boolean.Parse(ConfigurationManager.AppSettings["UseSSL"]);
            tracer.Trace($"UseSSL = {sourceUseSSL}...");


            try
            {
                tracer.Trace($"Connecting CRM...");

                _orgService = ConnectCrm(sourceUserName, sourceServer, sourcePassword, sourceDomain, sourcePort, sourceOrganizationName, sourceUseSSL);
                if (_orgService == null)
                {
                    tracer.Trace($"Connection was not successful...");
                    return null;
                }
                else
                {
                    tracer.Trace($"Connection successful...");

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
