using System.Collections.Generic;

namespace System.CommandLine;

public class CommandLineParser
{
    public string? Description { get; set; }

    private List<ArgumentBase> Arguments { get; set; } = new();
    private List<OptionBase> Options { get; set; } = new();

    public CommandLineParser(string? description = null)
    {
        Description = description;
    }

    public Argument AddArgument(string? name = null, char? shortName = null)
    {
        var arg = new Argument(name, shortName);
        Arguments.Add(arg);

        return arg;
    }

    public Argument<T> AddArgument<T>(string name, char? shortName = null) where T : IParsable<T>
    {
        var arg = new Argument<T>(name, shortName);
        Arguments.Add(arg);

        return arg;
    }

    public Option<T> AddOption<T>(string name, char? shortName) where T : IParsable<T>
    {
        var opt = new Option<T>(name, shortName);
        Options.Add(opt);

        return opt;
    }

    public EnumOption<T> AddEnumOption<T>(string name, char? shortName) where T : struct, Enum
    {
        var opt = new EnumOption<T>(name, shortName);
        Options.Add(opt);

        return opt;
    }

    public void Parse(string[] args)
    {
        if (args.Length < Arguments.Count)
        {
            throw new CommandLineException("Too few arguments");
        }

        var argIndex = 0;
        var argumentIndex = 0;

        while (argIndex < args.Length)
        {
            var nextToken = args[argIndex++].AsSpan();

            object parseResult = nextToken switch
            {
                ['-', char shortName] => ParseShortNameOption(shortName, args[argIndex++]),
                ['-', '-', .. ReadOnlySpan<char> name] => ParseNameOption(name.ToString(), args[argIndex++]),
                ReadOnlySpan<char> arg => ParseArgument(arg.ToString()),
            };

            if (parseResult is null)
            {
                throw new CommandLineException($"Unhandled error processing argument: {nextToken}");
            }
            
            OptionBase ParseShortNameOption(char shortName, string arg)
            {
                foreach (var opt in Options)
                {
                    if (opt.ShortName == shortName)
                    {
                        opt.Parse(arg);
                        return opt;
                    }
                }

                throw new CommandLineException($"Invalid option: {shortName}");
            }

            OptionBase ParseNameOption(string name, string arg)
            {
                foreach (var opt in Options)
                {
                    if (opt.Name == name)
                    {
                        opt.Parse(arg);
                        return opt;
                    }
                }

                throw new CommandLineException($"Invalid option: {name}");
            }

            ArgumentBase ParseArgument(string arg)
            {
                if (argumentIndex >= Arguments.Count)
                {
                    throw new CommandLineException($"Unexpected argument: {arg}");
                }

                var argument = Arguments[argumentIndex++];
                argument.Parse(arg);
                return argument;
            }
        }
    }
}
