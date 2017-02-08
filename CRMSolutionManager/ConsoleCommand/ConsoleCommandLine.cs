using ConsoleCommandLine.Tracer;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCommandLine
{
    public class ConsoleCommandLine
    {
        private static ConsoleCommandLine instance;
        public static ConsoleCommandLine Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConsoleCommandLine();
                }
                return instance;
            }
        }

        private List<Tuple<string, ConsoleCommandAttribute, Type>> Commands { get; set; }

        private Dictionary<string, string> Parameters = null;



        private ConsoleCommandLine()
        {
            Commands = new List<Tuple<string, ConsoleCommandAttribute, Type>>();
        }

        public void ShowHeader()
        {
            Console.Clear();

            Console.WriteLine("=============================================");
            Console.WriteLine("             CRM Solution Manager            ");
            Console.WriteLine("=============================================");
        }

        public void Initialize()
        {
            ShowHeader();

            //Iterate commands in current folder, in different assemblies
            RegisterCommands();
        }

        private void RegisterCommands()
        {
            List<Assembly> allAssemblies = new List<Assembly>();

            LoadAssemblies(allAssemblies);



            foreach (var assembly in allAssemblies)
            {
                var types = FindAllDerivedTypes<ConsoleCommandBase>(assembly)
                    .Where(w => w.GetCustomAttribute<ConsoleCommandAttribute>() != null);

                foreach (var type in types)
                {
                    var attributeValue = type.GetCustomAttribute<ConsoleCommandAttribute>();
                    Commands.Add(new Tuple<string, ConsoleCommandAttribute, Type>(attributeValue.Command, attributeValue, type));
                }
            }

        }

        public static List<Type> FindAllDerivedTypes<T>(Assembly assembly)
        {
            var derivedType = typeof(T);
            return assembly
                .GetTypes()
                .Where(t =>
                    t != derivedType &&
                    derivedType.IsAssignableFrom(t)
                    ).ToList();
        }

        private static void LoadAssemblies(List<Assembly> allAssemblies)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);


            foreach (string dll in Directory.GetFiles(path, "*.dll"))
                allAssemblies.Add(Assembly.LoadFile(dll));

            foreach (string dll in Directory.GetFiles(path, "*.exe"))
                allAssemblies.Add(Assembly.LoadFile(dll));
        }



        public void ProcessArgs(string[] args)
        {
            var tracer = new ConsoleTracer();

            Parse(args);

            string commandText = string.Empty;

            //Find the mathcing command through attributes
            var commandKey = Commands.Where(w => w.Item1.ToLowerInvariant() == args.FirstOrDefault().ToLowerInvariant())
                                     .FirstOrDefault();

            if (commandKey != null && !string.IsNullOrWhiteSpace(commandKey.Item1))
                commandText = commandKey.Item1;
            else
            {
                tracer.Trace("Command not found.");
                return;
            }



            //Execute the command
            ExecuteCommand(commandText, tracer, new Nothing());
        }

        private object ExecuteCommand(string commandText, ITracingService tracer, object input)
        {
            var command = Commands.FirstOrDefault(w => w.Item1.ToLowerInvariant() == commandText.ToLowerInvariant());


            if (command.Item2.DependentCommand != null)
            {
                var dependentConsoleCommandAttribute = command.Item2.DependentCommand.GetCustomAttribute<ConsoleCommandAttribute>();

                //TODO : Validate inputs

                input = ExecuteCommand(dependentConsoleCommandAttribute.Command, tracer, input);
            }

            dynamic commandDependentTypeInstance = Activator.CreateInstance(command.Item3);

            //TODO : Fill the similar properties with parameters
            var properties = command.Item3.GetProperties().Where(w => w.GetCustomAttribute<CommandLineAttributeBase>() != null);

            foreach (var property in properties)
            {
                var parameter = Parameters[property.Name];

                if (!string.IsNullOrWhiteSpace(parameter))
                {
                    try
                    {
                        //TODO : Based on property type, cast the value, ; StringInputAttribute - IntInputAttribute
                        property.SetValue(commandDependentTypeInstance, parameter);
                    }
                    catch (Exception ex)
                    {
                        //
                    }
                }
            }

            //TODO : Make tracer based on app.config
            return commandDependentTypeInstance.Execute(tracer, input);
        }

        public void ProcessLine(string line)
        {
            ProcessArgs(line.Split(' '));
        }

        private Dictionary<string, string> Parse(string[] args)
        {
            Parameters = new Dictionary<string, string>();

            int i = 0;

            while (i < args.Length)
            {
                if (args[i].Length > 1 && args[i][0] == '-')
                {
                    // The current string is a parameter name
                    string key = args[i].Substring(1, args[i].Length - 1);
                    string value = "";
                    i++;
                    if (i < args.Length)
                    {
                        if (args[i].Length > 0 && args[i][0] == '-')
                        {
                            // The next string is a new parameter, do not nothing
                        }
                        else
                        {
                            // The next string is a value, read the value and move forward
                            value = args[i];
                            i++;
                        }
                    }

                    Parameters[key] = value;
                }
                else
                {
                    i++;
                }
            }

            return Parameters;
        }

        public void Run(string[] args)
        {
            if (args == null || !args.Any())
            {
                do
                {
                    Console.Write(DateTime.Now.ToShortTimeString() + " > ");
                    var line = Console.ReadLine().Trim();
                    if (line.ToLowerInvariant() != "exit")
                        Instance.ProcessLine(line.Trim());
                    else
                        break;
                } while (true);
            }
            else
            {
                Instance.ProcessArgs(args);
            }
        }
    }
}
