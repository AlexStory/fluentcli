# FluentCli

A .Net fluent Api for writing command line applications. Heavily inspired by [commander](https://github.com/tj/commander.js), 
which was in turn inspired by [commander](https://github.com/commander-rb/commander)


## Example

```
static void Main(string[] args)
    {
        var app = new FluentCli.Program();

        app
            .Version("0.0.1")
            .AddFlag("-t, -test, --testing", "runs a test")
            .Run(args);
        
        if(app.Is("testing") {
          Console.WriteLine("Tested!");
        }

    }
```
