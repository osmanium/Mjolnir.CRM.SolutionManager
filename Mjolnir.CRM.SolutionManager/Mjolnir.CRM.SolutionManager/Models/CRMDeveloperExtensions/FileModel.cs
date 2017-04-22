using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mjolnir.CRM.SolutionManager.Models.CRMDeveloperExtensions
{
    [Serializable]
    public class FileModel
    {
        public Guid OrgId { get; set; }
        public string Path { get; set; }
        public Guid WebResourceId { get; set; }
        public string WebResourceName { get; set; }
        public string IsManaged { get; set; }
    }
}
