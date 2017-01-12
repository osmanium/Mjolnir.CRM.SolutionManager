using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class PluginContext
    {
        public ITracingService TracingService { get; private set; }
        public IPluginExecutionContext PluginExecutionContext { get; private set; }
        public IOrganizationService OrganizationService { get; private set; }
        public IServiceProvider ServiceProvider { get; private set; }

        public Guid UserId { get; private set; }

        public PluginContext(ITracingService TracingService, IPluginExecutionContext PluginExecutionContext, 
            IOrganizationService OrganizationService, IServiceProvider ServiceProvider, Guid UserId)
        {
            this.TracingService = TracingService;
            this.PluginExecutionContext = PluginExecutionContext;
            this.OrganizationService = OrganizationService;
            this.ServiceProvider = ServiceProvider;
            this.UserId = UserId;
        }
    }
}
