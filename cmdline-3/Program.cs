using System.CommandLine;

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
