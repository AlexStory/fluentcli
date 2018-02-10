using System.Collections.Generic;
using System.Linq;

namespace FluentCli.Domain
{
    public class Argument
    {
        public List<string> Flags;
        public string Name => Flags.Single(x => x.StartsWith("--")).TrimStart(new char[]{'-'});
        public string HelpText;
        internal ArgumentType type;
    }
}