using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class EntityManagerBase
    {
        internal CRMContext context = null;

        public EntityManagerBase(CRMContext context)
        {
            this.context = context;
        }

        

        public void HandleException(Exception ex)
        {
            context.TracingService.Trace(ex.Message + "\n" + ex.StackTrace + "\n");
        }
    }
}
