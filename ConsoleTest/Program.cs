using System;
using System.Linq;
using FluentCli;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new FluentCli.Program()
                .Version("0.0.1")
                .PrintErrors()
                .AddFlag("-t, -test, --testing", "runs a test")
                .AddOnce("-n, --name", "prints your name")
                .Run(args)
                .Build();

            if (app.testing) {
                Console.WriteLine("Tested!");
            }
            
            if (app.name != null) {
                Console.WriteLine($"Your name is {app.name}");
            }
        }
    }
}