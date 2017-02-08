using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCommandLine
{
    public class ConsoleCommandAttribute : Attribute
    {
        public string Command { get; set; }

        public string Desription { get; set; }

        public Type DependentCommand { get; set; }

    }
}
