using Microsoft.Xrm.Sdk;
using OG.CRM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OG.CRM.JavaScriptOperation
{
    public interface IJavaScriptOperationExecuter
    {
        string Execute(string input, PluginContext context);
    }
}
