using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaScriptOperation
{
    public class JavaScriptOperationResponseBase
    {
        public bool IsSuccesful { get; set; }
        public string ErrorMessage { get; set; }

        public JavaScriptOperationResponseBase()
        {
            IsSuccesful = false;

        }
    }
}
