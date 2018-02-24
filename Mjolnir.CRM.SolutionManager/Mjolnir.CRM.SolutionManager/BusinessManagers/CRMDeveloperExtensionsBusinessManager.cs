using Mjolnir.CRM.SolutionManager.Models.CRMDeveloperExtensions;
using System.IO;
using System.Xml.Serialization;

namespace Mjolnir.CRM.SolutionManager.BusinessManagers
{
    public class CrmDeveloperExtensionsBusinessManager
    {
        public WebResourceDeployerModel WebResourceDeployerModel { get; private set; }

        public CrmDeveloperExtensionsBusinessManager(string configPath)
        {
            WebResourceDeployerModel = ReadConfigFromFile(configPath);
        }

        public WebResourceDeployerModel ReadConfigFromFile(string configPath)
        {
            WebResourceDeployerModel result;

            var serializer = new XmlSerializer(typeof(WebResourceDeployerModel));

            using (var reader = new StreamReader(configPath)) { 
                result = (WebResourceDeployerModel)serializer.Deserialize(reader);
            }

            return result;
        }
    }
}
