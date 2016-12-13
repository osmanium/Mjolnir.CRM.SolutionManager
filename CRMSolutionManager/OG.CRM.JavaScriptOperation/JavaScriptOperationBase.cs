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
    public abstract class JavaScriptOperationBase<TRequest, TResponse> : IJavaScriptOperationExecuter
        where TResponse : JavaScriptOperationResponseBase, new()
        where TRequest : JavaScriptOperationRequestBase
    {
        public string Execute(string input, PluginContext context)
        {
            TRequest request = null;
            TResponse response = new TResponse();
            string responseJson = string.Empty;
            string errorMessage = string.Empty;
            bool isError = false;

            try
            {
                //Identify the request type
                request = DeserizalizeRequest(input);

                //Execute
                response = ExecuteInternal(request, context);
                response.IsSuccesful = true;
            }
            catch (Exception ex)
            {
                HandleErrorResponse(response, out errorMessage, out isError, ex);
            }

            //Serialize response
            return SerializeResponse(response);
        }

        private static void HandleErrorResponse(TResponse response, out string errorMessage, out bool isError, Exception ex)
        {
            isError = true;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(ex.Message);
            sb.AppendLine(ex.StackTrace);
            if (ex.InnerException != null)
            {
                sb.AppendLine(ex.InnerException.Message);
                if (ex.InnerException.StackTrace != null)
                {
                    sb.AppendLine(ex.InnerException.StackTrace);
                }
            }

            errorMessage = sb.ToString();

            response.ErrorMessage = errorMessage;
        }

        private string SerializeResponse(TResponse response)
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                using (JsonTextWriter jsonWriter = new JsonTextWriter(sw))
                {
                    var serializer = JsonSerializer.Create();

                    //Read the request
                    serializer.Serialize(jsonWriter, response);
                }
            }

            return sb.ToString();
        }

        private static TRequest DeserizalizeRequest(string input)
        {
            TRequest request;
            using (StringReader sr = new StringReader(input))
            {
                using (JsonTextReader jsonReader = new JsonTextReader(sr))
                {
                    var serializer = JsonSerializer.Create();

                    //Read the request
                    request = serializer.Deserialize<TRequest>(jsonReader);
                }
            }

            return request;
        }

        public abstract TResponse ExecuteInternal(TRequest req, PluginContext context);
    }
}
