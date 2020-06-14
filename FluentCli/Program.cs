using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using FluentCli.Domain;
using static FluentCli.Utility.ProgramHelper;

namespace FluentCli
{
    public class Program
    {
        private string _version;
        private string _name;
        private List<Argument> _arguments = new List<Argument>();
        private Dictionary<string, bool> _flags = new Dictionary<string, bool>();
        private Dictionary<string, string> _argOnce = new Dictionary<string, string>();
        private List<string> _remaining = new List<string>();
        private bool _isParsing = false;
        private bool _usedVersion = false;
        private bool _printErrors;
        private dynamic _results = new ExpandoObject();


        public Program()
        {
            AddFlag("-h, -?, --help" ,"Prints this text");
        }
        
        public Program Version(string version, string flags = "-V, --version")
        {
            _version = version;
            _usedVersion = true;
            AddFlag(flags, "Prints the current version of the application");
            return this;
        }

        public Program AppName(string name)
        {
            _name = name;
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
            SetResult(argument.Name, false);
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
            SetResult(argument.Name, null);
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
                        _remaining.Add(args[i]);
                        i ++;
                        continue;
                    }
                    
                    var arg = GetByFlag(args[i]);                    
                    switch (arg?.type)
                    {
                        case ArgumentType.Flag:
                            _flags[arg.Name] = true;
                            UpdateResult(arg.Name, true);
                            break;
                        case ArgumentType.Once:
                            if (ValidateArgAvailable(args, i + 1)) return this; 
                            _argOnce[arg.Name] = args[++i];
                            UpdateResult(arg.Name, args[i]);
                            break;
                        case null:
                            var flags = args[i].Trim('-').ToCharArray().Select(x => GetByFlag($"-{x}"));
                            if (flags.All(x => x != null && x.type == ArgumentType.Flag)){
                                foreach(var f in flags) {
                                    _flags[f.Name] = true;
                                    UpdateResult(f.Name, true);
                                }
                            }
                            break;
                    }

                }
                i++;
            }
            SetResult("Arguments", _remaining);

            if (Is("help"))
            {
                if (_name != null) {
                    Console.Write(_name + " ");
                }
                if(_usedVersion) {
                    Console.Write(_version);
                }
                Console.Write("\n");
                foreach (var arg in _arguments)
                {
                    var argStrings = string.Join(",", arg.Flags); 
                    Console.WriteLine($"{argStrings}\t\t{arg.HelpText}");
                }
            }

            if (_usedVersion && Is("version"))
            {
                Console.WriteLine(_version);
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

        public List<string> Arguments(){
            return _remaining;
        }

        public Program PrintErrors()
        {
            _printErrors = true;
            return this;
        }

        public dynamic Build() {
            return _results;
        }

        // PRIVATE METHODS
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

        private void SetResult(string key, object value) {
            (_results as IDictionary<string, object>).Add(SanitizeName(key), value);
        }

        private void UpdateResult(string key, object value) {
            (_results as IDictionary<string, object>)[SanitizeName(key)] = value;
        }


    }
}