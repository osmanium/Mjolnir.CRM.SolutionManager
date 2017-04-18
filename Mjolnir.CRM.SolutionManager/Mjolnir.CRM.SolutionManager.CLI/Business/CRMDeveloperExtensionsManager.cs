using Mjolnir.CRM.SolutionManager.CLI.CRMDeveloperExtensions.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Mjolnir.CRM.SolutionManager.CLI.Business
{
    public class CRMDeveloperExtensionsManager
    {
        public WebResourceDeployerModel WebResourceDeployerModel { get; private set; }

        public CRMDeveloperExtensionsManager(string configPath)
        {
            this.WebResourceDeployerModel = ReadConfigFromFile(configPath);
        }

        private WebResourceDeployerModel ReadConfigFromFile(string configPath)
        {
            WebResourceDeployerModel result = null;

            XmlSerializer serializer = new XmlSerializer(typeof(WebResourceDeployerModel));

            using (StreamReader reader = new StreamReader(configPath)) { 
                result = (WebResourceDeployerModel)serializer.Deserialize(reader);
            }

            return result;
        }



    }
}
