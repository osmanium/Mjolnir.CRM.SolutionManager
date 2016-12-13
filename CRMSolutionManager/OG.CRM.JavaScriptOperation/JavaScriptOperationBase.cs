using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using OG.CRM.Common;
using Newtonsoft.Json;
using System.IO;

namespace OG.CRM.JavaScriptOperation
{
    public abstract class JavaScriptOperationBase<TRequest, TResponse> : IJavaScriptOperationExecuter<TRequest, TResponse>
        where TResponse : new()
        where TRequest : JavaScriptOperationRequestBase
    {
        public TResponse Execute(string input, PluginContext context)
        {
            TRequest request = null;

            //Identify the request type
            using (StringReader sr = new StringReader(input))
            {
                using (JsonTextReader jsonReader = new JsonTextReader(sr))
                {
                    var serializer = JsonSerializer.Create();

                    //Read the request
                    request = serializer.Deserialize<TRequest>(jsonReader);
                }
            }

            //Execute
            return ExecuteInternal(request, context);
        }

        public abstract TResponse ExecuteInternal(TRequest req, PluginContext context);
    }
}
