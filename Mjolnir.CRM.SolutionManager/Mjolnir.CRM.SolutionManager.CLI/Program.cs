using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using Mjolnir.ConsoleCommandLine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mjolnir.CRM.SolutionManager.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            var commandLine = Mjolnir.ConsoleCommandLine.ConsoleCommandLine.Instance;
            commandLine.HeaderAction = () =>
            {
                Console.WriteLine("==========================================================================");
                
                Console.WriteLine(@" __  __        _    ____    _        _   _   _____   _____  
|  \/  |      | |  / __ \  | |      | \ | | |_   _| |  __ \ 
| \  / |      | | | |  | | | |      |  \| |   | |   | |__) |
| |\/| |  _   | | | |  | | | |      | . ` |   | |   |  _  / 
| |  | | | |__| | | |__| | | |____  | |\  |  _| |_  | | \ \ 
|_|  |_|  \____/   \____/  |______| |_| \_| |_____| |_|  \_\                      
                                    ");
                Console.WriteLine("CRM SOLUTION MANAGER        ");
                Console.WriteLine("==========================================================================");
                Console.WriteLine("");
            };


            commandLine.Initialize();

            commandLine.Run(args);
        }
    }
}
