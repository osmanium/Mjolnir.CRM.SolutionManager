using Common;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ConsoleCommandLine;

namespace CRMSolutionManager.CLI
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            var commandLine = ConsoleCommandLine.ConsoleCommandLine.Instance;

            commandLine.Initialize();
            commandLine.Run(args);
        }
    }
}
