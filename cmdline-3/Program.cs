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

var parser = new CommandLineParser("Get the URL for a GitHub issue or pull request");

var typeOption = parser.AddEnumOption<ItemType>("type", 't');
var number = parser.AddArgument<uint>("number");
parser.Parse(args);

var slug = typeOption.Value switch
{
    ItemType.Issue => "issues",
    ItemType.PullRequest => "pull",
    _ => throw new CommandLineException("Unrecognized item type: {typeOption.Value}"),
};

Console.WriteLine($"https://github.com/dotnet/runtime/{slug}/{number.Value}");

public enum ItemType : byte
{
    Issue,
    PullRequest
}
