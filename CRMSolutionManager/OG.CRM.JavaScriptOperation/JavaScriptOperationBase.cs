using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace OG.CRM.JavaScriptOperation
{
    public abstract class JavaScriptOperationBase<TRequest, TResponse> : IJavaScriptOperationExecuter<TRequest>
        where TResponse : new()
        where TRequest : JavaScriptOperationRequestBase
    {
        public void execute(TRequest req, ITracingService tracer)
        {
            //TODO: execute internal
        }

        public abstract TResponse ExecuteInternal(TResponse req);
    }
}
