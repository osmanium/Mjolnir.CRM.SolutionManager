using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mjolnir.CRM.Common
{
    public class CRMContext
    {
        public ITracingService TracingService { get; private set; }
        public IPluginExecutionContext PluginExecutionContext { get; private set; }
        public IOrganizationService OrganizationService { get; private set; }
        public IServiceProvider ServiceProvider { get; private set; }

        public Guid UserId { get; private set; }

        public CRMContext(IOrganizationService OrganizationService, Guid UserId,
            ITracingService TracingService, IPluginExecutionContext PluginExecutionContext = null, IServiceProvider ServiceProvider = null)
        {
            this.TracingService = TracingService;
            this.PluginExecutionContext = PluginExecutionContext;
            this.OrganizationService = OrganizationService;
            this.ServiceProvider = ServiceProvider;
            this.UserId = UserId;
        }
    }
}
