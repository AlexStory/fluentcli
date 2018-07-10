using System.Collections.Generic;
using System.Linq;

namespace FluentCli.Utility
{
    public static class ProgramHelper
    {
        public static List<string> ParseFlags(string input)
        {
            return input.Split(',').Select(x => x.Trim()).ToList();
        }

        public static string SanitizeName(string name) {
            return string.Join("", name.Split('-').Select(x => $"{x.First().ToString().ToUpper()}{x.Substring(1)}"));
        }
    }
}