using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OG.CRM.Common
{
    public abstract class PluginBase : IPlugin
    {
        public ITracingService tracingService { get; private set; }
        

        public void Execute(IServiceProvider serviceProvider)
        {
            this.tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            ExecuteInternal(this.tracingService);

        }

        public abstract void ExecuteInternal(ITracingService tracingService);
    }
}
