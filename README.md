# FluentCli

A .NET fluent Api for writing command line applications. Heavily inspired by [commander](https://github.com/tj/commander.js), 
which was in turn inspired by [commander](https://github.com/commander-rb/commander)


## Example

```csharp
static void Main(string[] args)
{
    var app = new FluentCli.Program();

    app
        .Version("0.0.1")
        .PrintErrors()
        .AddFlag("-t, -test, --testing", "runs a test")
        .AddOnce("-n, --name", "prints your name")
        .Run(args);

    if (app.Is("testing") {
      Console.WriteLine("Tested!");
    }
    
    if (app.Get("name") != null {
      Console.WriteLine($"Your name is {app.Get("name")}");
    }

}
```

## Api

### AddFlag(string flags, string helpText)
###### flags

String of all flags that you want to correspond to this flag. Seperate them by commas.
One flag must start with "--" and only one.

###### helpText

The text that is printed to the command line when a user asks for help.

### AddOnce(string flags, string helpText)
###### flags

String of all flags that you want to correspond to this flag. Seperate them by commas.
One flag must start with "--" and only one.

###### helpText

The text that is printed to the command line when a user asks for help.

### Is(string flagName)
###### flagName
The long flag of the argument you want to query.

returns wether the given flag was entered by the user. Will look at all flag variations for the entered flag. ie `Is("help")`
will look for '-h', '-?', or '--help'.

### Get(string flagName)
###### flagName
The long flag of the argument you want to query. Will look at all flag variations for the entered flag. ie `Get("argument")`
will look for '-a', '-arg', or '--argument'.

### Version(string version, [string flags])
Shortcut for adding a version argument. defaults to "-V" and "--version".

### PrintErrors()
If this function is called, errors generated by users will be printed directly to the console. If not called exceptions will be thrown so you can handle user interaction yourself.

returns the argument entered after the flag. 

## Todo
- Testing, more of it. Unit tests, integration tests.
- Better Docs.
- Flag types: need to parse more than just booleans. Want to parse ~single values~ and lists.
- Possibly convert scalar types to take even more load off of users.
- Refactoring main program file. Too much in it. and Run() method could be lighter.
- Multiflag ie. "-abc" can be the same as "-a -b -c"
