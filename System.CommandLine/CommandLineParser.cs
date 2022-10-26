using System.Collections.Generic;

namespace System.CommandLine;

public class CommandLineParser
{
    public string? Description { get; set; }

    private List<ArgumentBase> Arguments { get; set; } = new();

    public CommandLineParser(string description)
    {
        Description = description;
    }

    public Argument AddArgument(string name)
    {
        var arg = new Argument(name);
        Arguments.Add(arg);

        return arg;
    }

    public Argument<T> AddArgument<T>(string name) where T : IParsable<T>
    {
        var arg = new Argument<T>(name);
        Arguments.Add(arg);

        return arg;
    }

    public void Parse(string[] args)
    {
        if (args.Length < Arguments.Count)
        {
            throw new CommandLineException("Too few arguments");
        }

        var i = 0;

        foreach (var arg in Arguments)
        {
            arg.Parse(args[i++]);
        }
    }
}
