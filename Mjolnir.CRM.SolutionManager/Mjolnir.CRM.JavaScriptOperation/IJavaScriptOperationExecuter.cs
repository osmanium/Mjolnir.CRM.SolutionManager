using Microsoft.Xrm.Sdk;
using Mjolnir.CRM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mjolnir.CRM.JavaScriptOperation
{
    public interface IJavaScriptOperationExecuter
    {
        string Execute(string input, CRMContext context);
    }
}
