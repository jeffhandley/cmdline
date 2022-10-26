using System.CommandLine;

/*
    1. `dotnet watch`: (1) "run"
    2. `dotnet watch run`: (1) "run"
    3. `dotnet watch run Hello`: (1) "Hello"
    4. `dotnet run`: (0)
    5. `dotnet run Hello`: (1) "Hello"
    6. `dotnet run -- Hello`: (1) "Hello"
    7. `dotnet watch run -- Hello`: (1) "Hello"
    8. `dotnet watch -- Hello`: Error from dotnet.exe
*/

var parser = new CommandLineParser("Show the month name given a number");
var month = parser.AddArgument<int>("month");
parser.Parse(args);

var name = month.Value switch
{
    1 => "January",
    2 => "February",
    3 => "March",
    4 => "April",
    5 => "May",
    6 => "June",
    7 => "July",
    8 => "August",
    9 => "September",
    10 => "October",
    11 => "November",
    12 => "December",
    _ => throw new CommandLineException($"Unrecognized value: {month.Value} "),
};

Console.WriteLine($"{month.Value}: {name}");
