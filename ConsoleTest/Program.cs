﻿using System;
using System.Linq;
using FluentCli;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new FluentCli.Program()
                .AppName("Console Test")
                .Version("0.0.1")
                .PrintErrors()
                .AddFlag("-t, -test, --testing", "runs a test")
                .AddOnce("-n, --name", "prints your name")
                .AddFlag("-l, --list-this", "lists the thing")
                .Run(args)
                .Build();

            if (app.Testing) {
                Console.WriteLine("Tested!");
            }
            
            if (app.Name != null) {
                Console.WriteLine($"Your name is {app.Name}");
            }

            if (app.ListThis) {
                Console.WriteLine($"Listing stuff");
            } 
        }
    }
}