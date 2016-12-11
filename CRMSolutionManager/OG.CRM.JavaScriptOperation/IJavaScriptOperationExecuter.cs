using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OG.CRM.JavaScriptOperation
{
    public interface IJavaScriptOperationExecuter<TRequest>
    {
        void execute(TRequest req, ITracingService tracer);
    }
}
