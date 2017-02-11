using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mjolnir.CRM.JavaScriptOperation
{
    public interface IJavaScriptOperationResponse
    {
        bool IsSuccesful { get; set; }
        string ErrorMessage { get; set; }
    }
}
