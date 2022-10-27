using System.CommandLine;

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
