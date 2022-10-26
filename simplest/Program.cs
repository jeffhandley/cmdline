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

var parser = new CommandLineParser("Show a team member's name");
var alias = parser.AddArgument("alias");
parser.Parse(args);

var name = alias.Value.ToLower() switch
{
    "adamsitnik" => "Adam Sitnik",
    "buyaa-n" => "Buyaa Namnan",
    "carlossanlop" => "Carlos Sanchez Lopez",
    "jozkee" => "David Cantu",
    "dakersnar" => "Drew Kersnar",
    "eiriktsarpalis" => "Eirik Tsarpalis",
    "jeffhandley" => "Jeff Handley",
    "bartonjs" => "Jeremy Barton",
    "krwq" => "Krzysztof Wicher",
    "grabyourpitchforks" => "Levi Broderick",
    "tannergooding" => "Tanner Gooding",
    _ => throw new CommandLineException($"Unrecognized alias: {alias.Value} "),
};

Console.WriteLine($"{alias.Value}: {name}");
