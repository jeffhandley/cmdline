using System.CommandLine;

var parser = new CommandLineParser();
var alias = parser.AddArgument();
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
