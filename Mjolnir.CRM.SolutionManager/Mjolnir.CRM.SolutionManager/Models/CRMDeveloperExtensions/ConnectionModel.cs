using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mjolnir.CRM.SolutionManager.Models.CRMDeveloperExtensions
{
    [Serializable]
    public class ConnectionModel
    {
        public string Name { get; set; }
        public Guid OrgId { get; set; }
        public string ConnectionString { get; set; }
        public Version Version { get; set; }
    }
}
