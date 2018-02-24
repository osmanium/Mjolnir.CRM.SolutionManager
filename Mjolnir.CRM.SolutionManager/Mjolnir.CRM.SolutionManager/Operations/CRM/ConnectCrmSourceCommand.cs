using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using Mjolnir.ConsoleCommandLine;
using Mjolnir.CRM.Core;
using Mjolnir.CRM.Core.Loggers;
using System;
using System.Configuration;
using System.Threading.Tasks;
using CommandLine;
using Mjolnir.ConsoleCommandLine.Tracer;

namespace Mjolnir.CRM.SolutionManager.Operations.CRM
{
    [Verb("Connect-CrmSource")]
    public class ConnectCrmSourceCommand : ConsoleCommandBase
    {
        private static CrmServiceClient ConnectCrm(string connectionString)
        {
            var sourceCrmServiceClient = new CrmServiceClient(connectionString);
            return sourceCrmServiceClient;
        }

        /// <inheritdoc />
        public override async Task<object> ExecuteCommand(ITracingService tracer, object input)
        {
            return Task.Run(() =>
            {
                OrganizationServiceProxy orgService;

                tracer.Trace($"Reading CRM connection configuration...");

                var sourceOrgConnectionString = ConfigurationManager.AppSettings["SourceOrgConnectionString"];
                tracer.Trace($"sourceOrgConnectionString = {sourceOrgConnectionString}...");

                try
                {
                    tracer.Trace($"Connecting CRM...");

                    var client = ConnectCrm(sourceOrgConnectionString);
                    var timeOutinSeconds = int.Parse(ConfigurationManager.AppSettings["TimeOutInMinutes"]);
                    client.OrganizationServiceProxy.Timeout = TimeSpan.FromSeconds(timeOutinSeconds);

                    orgService = client.OrganizationServiceProxy;
                    if (orgService == null)
                    {
                        tracer.Trace($"Connection was not successful...");
                        return null;
                    }
                    else
                    {
                        tracer.Trace($"Connection successful...");

                        if (int.TryParse(ConfigurationManager.AppSettings[Constants.TimeOutInMinutes], out var timeOutInMinutes))
                        {
                            orgService.Timeout = new TimeSpan(0, timeOutInMinutes, 0);
                        }
                        else
                        {
                            orgService.Timeout = new TimeSpan(0, 30, 0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    HandleCommandException(tracer, ex);
                    return null;
                }

                return new CrmContext(orgService, orgService.CallerId, new CrmLogger(new CrmExternalTracer()));
            });
        }
    }
}
