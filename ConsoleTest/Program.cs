using System;
using FluentCli;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new FluentCli.Program();
                
            app
                .Version("0.0.1")
                .PrintErrors()
                .AddOnce("-a, --arg", "Argument to run")
                .Run(args);

            if (app.Is("version"))
            {
                Console.WriteLine("version queried");
            }

            if (app.Get("arg") != null)
            {
                Console.WriteLine($"The arg was: {app.Get("arg")}");
            }
        }
    }
}