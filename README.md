# FluentCli

A .Net fluent Api for writing command line applications. Heavily inspired by [commander](https://github.com/tj/commander.js), 
which was in turn inspired by [commander](https://github.com/commander-rb/commander)


## Example

```csharp
static void Main(string[] args)
{
    var app = new FluentCli.Program();

    app
        .Version("0.0.1")
        .AddFlag("-t, -test, --testing", "runs a test")
        .Run(args);

    if (app.Is("testing") {
      Console.WriteLine("Tested!");
    }

}
```

## Api

#### AddFlag(string flags, string helpText)
###### flags

String of all flags that you want to correspond to this flag. Seperate them by commas.
One flag must start with "--" and only one.

###### helpText

The text that is printed to the command line when a user asks for help.

#### Is(string flagName)
###### flagName
The long flag of the argument you want to query.

returns wether the given flag was entered by the user. Will look at all flag variations for the entered flag. ie `Is("help")`
will look for '-h', '-?', or '--help'.

## Todo
- Testing, more of it. Unit tests, integration tests.
- Better Docs.
- Flag types: need to parse more than just booleans. Want to parse single values and lists.
- Possibly convert scalar types to take even more load off of users.
