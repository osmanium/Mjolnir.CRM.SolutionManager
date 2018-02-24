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
    [Verb("Connect-CrmTarget")]
    public class ConnectCrmTargetCommand : ConsoleCommandBase
    {
        private static CrmServiceClient ConnectCrm(string connectionString)
        {
            var sourceCrmServiceClient = new CrmServiceClient(connectionString);
            return sourceCrmServiceClient;
        }

        public override async Task<object> ExecuteCommand(ITracingService tracer, object input)
        {
            return Task.Run(() =>
            {
                OrganizationServiceProxy orgService;

                tracer.Trace($"Reading Target CRM connection configuration...");

                var targetOrgConnectionString = ConfigurationManager.AppSettings["TargetOrgConnectionString"];
                tracer.Trace($"sourceOrgConnectionString = {targetOrgConnectionString}...");


                try
                {
                    tracer.Trace($"Connecting CRM...");

                    var client = ConnectCrm(targetOrgConnectionString);
                    var timeOutinSeconds = int.Parse(ConfigurationManager.AppSettings["TimeOutInMinutes"]);
                    client.OrganizationServiceProxy.Timeout = TimeSpan.FromSeconds(timeOutinSeconds);

                    orgService = client.OrganizationServiceProxy;
                    if (orgService == null)
                    {
                        tracer.Trace($"Target Connection was not successful...");
                        return null;
                    }
                    else
                    {
                        tracer.Trace($"Target Connection successful...");

                        if (int.TryParse(ConfigurationManager.AppSettings[Constants.TimeOutInMinutes],
                                         out var timeOutInMinutes))
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
