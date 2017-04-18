using Mjolnir.CRM.SolutionManager.CLI.Models.CRMDeveloperExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Mjolnir.CRM.SolutionManager.CLI.CRMDeveloperExtensions.Models
{
    [Serializable]
    [System.Xml.Serialization.XmlRoot("WebResourceDeployer")]
    public class WebResourceDeployerModel
    {
        [XmlArray("Connections")]
        [XmlArrayItem("Connection", typeof(ConnectionModel))]
        public ConnectionModel[] Connections { get; set; }


        [XmlArray("Files")]
        [XmlArrayItem("File", typeof(FileModel))]
        public FileModel[] Files { get; set; }
    }
}
