using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public abstract class PluginBase : IPlugin
    {
        protected CRMContext PluginContext { get; private set; }

        public void Execute(IServiceProvider serviceProvider)
        {
            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            var pluginExecutionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var organizationServiceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            var organizationService = organizationServiceFactory.CreateOrganizationService(pluginExecutionContext.UserId);

            this.PluginContext = new CRMContext(tracingService, pluginExecutionContext, organizationService, serviceProvider, pluginExecutionContext.UserId);

            
            ExecuteInternal(this.PluginContext);

        }

        public abstract void ExecuteInternal(CRMContext pluginContext);
    }
}
