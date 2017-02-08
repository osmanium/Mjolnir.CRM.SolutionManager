using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.EntityManagers
{
    public class EntityManager : EntityManagerBase
    {
        public EntityManager(CRMContext context)
            : base(context)
        { }

        public void DeleteEntity()
        {

        }
    }
}
