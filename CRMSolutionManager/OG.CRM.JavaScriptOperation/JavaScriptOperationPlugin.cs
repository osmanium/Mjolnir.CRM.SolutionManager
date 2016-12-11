using Microsoft.Xrm.Sdk;
using OG.CRM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OG.CRM.JavaScriptOperation
{
    public class JavaScriptOperationPlugin : PluginBase
    {
        public override void ExecuteInternal(ITracingService tracingService)
        {
            tracingService.Trace("trace started");

            try
            {
                tracingService.Trace("initiating get type");
                Type t = Type.GetType("OG.CRM.CRMSolutionManager.ConvertAllPatchesToSolutions", false, true);

                tracingService.Trace("initiating create instance");
                IOperation obj = (IOperation)Activator.CreateInstance(t);

                if (obj != null)
                    obj.execute(tracingService);
                else
                    tracingService.Trace("null");


                tracingService.Trace("done");
            }
            catch (Exception ex)
            {
                tracingService.Trace("exception occured");
                throw;
            }
            finally
            {
                tracingService.Trace("trace end");
            }
        }
    }
}