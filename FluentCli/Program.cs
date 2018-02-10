﻿using System;
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
        private Dictionary<string, string> _argOnce = new Dictionary<string, string>();
        private bool _isParsing;
        private bool _printErrors;


        public Program()
        {
            AddFlag("-h, -?, --help" ,"Prints this text");
        }
        
        public Program Version(string version, string flags = "-V, --version")
        {
            _version = version;
            AddFlag(flags, "Prints the current version of the application");
            return this;
        }

        public Program AddFlag(string flags, string helpText)
        {
            var parsedFlags = ParseFlags(flags);
            ValidateFlags(parsedFlags);
            
            var argument = new Argument
            {
                Flags = parsedFlags,
                HelpText = helpText,
                type = ArgumentType.Flag
            };

            _flags[argument.Name] = false;
            _arguments.Add(argument);
            return this;
        }

        public Program AddOnce(string flags, string helpText)
        {
            var parsedFlags = ParseFlags(flags);
            ValidateFlags(parsedFlags);

            var argument = new Argument
            {
                Flags = parsedFlags,
                HelpText = helpText,
                type = ArgumentType.Once
            };

            _argOnce[argument.Name] = null;
            _arguments.Add(argument);
            return this;
        }

        public Program Run(string[] args)
        {
            var i = 0;
            while (i < args.Length)
            {
                if (!_isParsing)
                {
                    if (!args[i].StartsWith("-"))
                    {
                        HandleError($"Unexpected value: {args[i]}");
                    }
                    
                    var arg = GetByFlag(args[i]);
                    
                    switch (arg.type)
                    {
                        case ArgumentType.Flag:
                            _flags[arg.Name] = true;
                            break;
                        case ArgumentType.Once:
                            if (ValidateArgAvailable(args, i + 1)) return this; 
                            _argOnce[arg.Name] = args[++i];
                            break;
                    }

                    i++;
                }
            }

            if (Is("help"))
            {
                foreach (var arg in _arguments)
                {
                    var argStrings = string.Join(",", arg.Flags); 
                    Console.WriteLine($"{argStrings}\t\t{arg.HelpText}");
                }
            }
            
            return this;
        }

        public bool Is(string name)
        {
            return _flags[name];
        }

        public string Get(string name)
        {
            return _argOnce[name];
        }

        public Program PrintErrors()
        {
            _printErrors = true;
            return this;
        }

        private void HandleError(string ErrorString)
        {
            if (_printErrors)
            {
                Console.Error.WriteLine(ErrorString);
            }
            else
            {
                throw new Exception(ErrorString);
            }
        }

        private Argument GetByFlag(string flag)
        {
            return _arguments.FirstOrDefault(x => x.Flags.Contains(flag));
        }

        private void ValidateFlags(List<string> flags)
        {
            if (!flags.Any(x => x.StartsWith("--")))
            {
                throw new ArgumentException($"Need long termed flag is: \"--flag\" found {flags}");
            }

            if (flags.Count(x => x.StartsWith("--")) > 1)
            {
                throw new ArgumentException($"Found multiple long termed flags {flags}");
            }
        }

        private bool ValidateArgAvailable(string[] args, int index)
        {
            var result = false;
            if (index < args.Length) return result;
            
            HandleError($"Expected argument after \"{args[index - 1]}\"");
            result = true;

            return result;
        }


    }
}