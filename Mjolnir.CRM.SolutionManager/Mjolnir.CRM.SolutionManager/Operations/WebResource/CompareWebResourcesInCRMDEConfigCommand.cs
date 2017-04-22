using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Mjolnir.ConsoleCommandLine;
using Mjolnir.ConsoleCommandLine.InputAttributes;
using Mjolnir.CRM.Core;
using Mjolnir.CRM.Sdk.Entities;
using Mjolnir.CRM.SolutionManager.BusinessManagers;
using Mjolnir.CRM.SolutionManager.Operations.CRM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mjolnir.CRM.SolutionManager.Operations.Solution.WebResource
{

    [ConsoleCommandAttribute(
        Command = "CompareWebResourcesInCRMDEConfig",
        Desription = "",
        DependentCommand = typeof(CrmConnectCommand))]
    public class CompareWebResourcesInCRMDEConfigCommand : ConsoleCommandBase
    {
        [StringInput(Description = "CRMDeveloperExtensionsConfig Path", IsRequired = true)]
        public string CRMDeveloperExtensionsConfigPath { get; set; }

        public override object ExecuteCommand(ITracingService tracer, object input)
        {
            try
            {
                tracer.Trace($"Reading CRMDeveloperExtensionsConfigPath {CRMDeveloperExtensionsConfigPath}...");

                CrmContext ctx = (CrmContext)input;

                var webResourceManager = new Core.EntityManagers.WebResourceManager(ctx);
                var crmDeveloperExtensionsManager = new CRMDeveloperExtensionsBusinessManager(CRMDeveloperExtensionsConfigPath);


                tracer.Trace($"Getting web resource with contents");
                var webResourcesWithContents = webResourceManager.GetWebResourcesContentsByIds(crmDeveloperExtensionsManager
                                                                    .WebResourceDeployerModel
                                                                    .Files
                                                                        .Select(s => s.WebResourceId.ToString()).ToArray());

                
                tracer.Trace($"Comparing web resources with local files");
                foreach (var webResource in webResourcesWithContents)
                {
                    //TODO : Convert webResource Entity to WebResourceEntity type

                    //TODO : Read local file content and convert to base64
                    string localWebResourceContent = null;
                    string localWebResourceContentBase64 = null;

                    var webResourceFile = crmDeveloperExtensionsManager.WebResourceDeployerModel.Files.Where(w => w.WebResourceId == webResource.Id).First();

                    using (StreamReader sr = new StreamReader(webResourceFile.Path))
                    {
                        localWebResourceContent = sr.ReadToEnd();

                        //localWebResourceContentBase64 = localWebResourceContent.
                    }



                    //TODO : Compare with web resource fetched from CRM

                    //TODO : If it is not equal, write to console
                }


                tracer.Trace($"Successfully web resources compared");
                return true;
            }
            catch (Exception ex)
            {
                HandleCommandException(tracer, ex);
                return false;
            }
        }
    }
}
