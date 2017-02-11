using Mjolnir.CRM.JavaScriptOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mjolnir.CRM.SolutionManager.Infrastructure.ConvertPatchesToSolution
{
    public class ConvertPatchesToSolutionRequest: IJavaScriptOperationRequest
    {
        public string[] SelectedSolutionIds { get; set; }
    }
}
