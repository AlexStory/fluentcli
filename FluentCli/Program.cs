using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using FluentCli.Domain;
using static FluentCli.Utility.ProgramHelper;

namespace FluentCli
{
    public class Program
    {
        private string _version;
        private List<Argument> _arguments = new List<Argument>();
        private Dictionary<string, bool> _flags = new Dictionary<string, bool>();
        private bool _isParsing;


        public Program()
        {
            AddFlag("-h, -?, --help" ,"Prints this text");
        }
        
        public Program Version(string version)
        {
            _version = version;
            AddFlag("-V, --version", "Prints the current version of the application");
            return this;
        }

        public Program AddFlag(string flags, string helpText)
        {
            var parsedFlags = ParseFlags(flags);
            if (!parsedFlags.Any(x => x.StartsWith("--")))
            {
                throw new ArgumentException($"Need long termed flag is: \"--flag\" found {flags}");
            }

            if (parsedFlags.Count(x => x.StartsWith("--")) > 1)
            {
                throw new ArgumentException($"Found multiple long termed flags {flags}");
            }
            
            var argument = new Argument
            {
                Flags = ParseFlags(flags),
                HelpText = helpText,
                type = ArgumentType.Flag
            };

            _flags[argument.Name] = false;
            _arguments.Add(argument);
            return this;
        }

        public Program Run(string[] args)
        {
            var i = 0;
            while (i < args.Length)
            {
                var arg = GetByFlag(args[i]);
                if (arg.type == ArgumentType.Flag)
                {
                    _flags[arg.Name] = true;
                }

                i++;
            }

            if (Is("help"))
            {
                foreach (var arg in _arguments)
                {
                    string argStrings = string.Join(",", arg.Flags); 
                    Console.WriteLine($"{argStrings}\t\t{arg.HelpText}");
                }
            }
            
            return this;
        }

        public bool Is(string name)
        {
            return _flags[name];
        }

        public Argument GetByFlag(string flag)
        {
            return _arguments.FirstOrDefault(x => x.Flags.Contains(flag));
        }


    }
}