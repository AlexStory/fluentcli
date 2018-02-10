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
                .Run(args);

            if (app.Is("version"))
            {
                Console.WriteLine("version queried");
            }
        }
    }
}