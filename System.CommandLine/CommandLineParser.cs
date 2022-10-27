using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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

    public Argument AddArgument() => AddArgument(new string[] { }, new char[] { });
    public Argument AddArgument(char shortName) => AddArgument(new string[] { }, new [] { shortName });
    public Argument AddArgument(char[] shortNames) => AddArgument(new string[] { }, shortNames);
    public Argument AddArgument(string name) => AddArgument(new[] { name }, new char[] { });
    public Argument AddArgument(string[] names) => AddArgument(names, new char[] { });
    public Argument AddArgument(string name, char shortName) => AddArgument(new[] { name }, new[] { shortName });
    public Argument AddArgument(string[] names, char shortName) => AddArgument(names, new[] { shortName });
    public Argument AddArgument(string name, char[] shortNames) => AddArgument(new[] { name }, shortNames);
    public Argument AddArgument(string[] names, char[] shortNames)
    {
        var arg = new Argument(names, shortNames);
        Arguments.Add(arg);

        return arg;
    }

    public Argument<T> AddArgument<T>() where T : IParsable<T> => AddArgument<T>(new string[] { }, new char[] { });
    public Argument<T> AddArgument<T>(char shortName) where T : IParsable<T> => AddArgument<T>(new string[] { }, new[] { shortName });
    public Argument<T> AddArgument<T>(char[] shortNames) where T : IParsable<T> => AddArgument<T>(new string[] { }, shortNames);
    public Argument<T> AddArgument<T>(string name) where T : IParsable<T> => AddArgument<T>(new[] { name }, new char[] { });
    public Argument<T> AddArgument<T>(string[] names) where T : IParsable<T> => AddArgument<T>(names, new char[] { });
    public Argument<T> AddArgument<T>(string name, char shortName) where T : IParsable<T> => AddArgument<T>(new[] { name }, new[] { shortName });
    public Argument<T> AddArgument<T>(string[] names, char shortName) where T : IParsable<T> => AddArgument<T>(names, new[] { shortName });
    public Argument<T> AddArgument<T>(string name, char[] shortNames) where T : IParsable<T> => AddArgument<T>(new[] { name }, shortNames);
    public Argument<T> AddArgument<T>(string[] names, char[] shortNames) where T : IParsable<T>
    {
        var arg = new Argument<T>(names, shortNames);
        Arguments.Add(arg);

        return arg;
    }

    public Option AddOption(char shortName) => AddOption(new string[] { }, new[] { shortName });
    public Option AddOption(char[] shortNames) => AddOption(new string[] { }, shortNames);
    public Option AddOption(string name) => AddOption(new[] { name }, new char[] { });
    public Option AddOption(string[] names) => AddOption(names, new char[] { });
    public Option AddOption(string name, char shortName) => AddOption(new[] { name }, new[] { shortName });
    public Option AddOption(string[] names, char shortName) => AddOption(names, new[] { shortName });
    public Option AddOption(string name, char[] shortNames) => AddOption(new[] { name }, shortNames);
    public Option AddOption(string[] names, char[] shortNames)
    {
        var opt = new Option(names, shortNames);
        Options.Add(opt);

        return opt;
    }

    public Option<T> AddOption<T>(char shortName) where T : IParsable<T> => AddOption<T>(new string[] { }, new[] { shortName });
    public Option<T> AddOption<T>(char[] shortNames) where T : IParsable<T> => AddOption<T>(new string[] { }, shortNames);
    public Option<T> AddOption<T>(string name) where T : IParsable<T> => AddOption<T>(new[] { name }, new char[] { });
    public Option<T> AddOption<T>(string[] names) where T : IParsable<T> => AddOption<T>(names, new char[] { });
    public Option<T> AddOption<T>(string name, char shortName) where T : IParsable<T> => AddOption<T>(new[] { name }, new[] { shortName });
    public Option<T> AddOption<T>(string[] names, char shortName) where T : IParsable<T> => AddOption<T>(names, new[] { shortName });
    public Option<T> AddOption<T>(string name, char[] shortNames) where T : IParsable<T> => AddOption<T>(new[] { name }, shortNames);
    public Option<T> AddOption<T>(string[] names, char[] shortNames) where T : IParsable<T>
    {
        var opt = new Option<T>(names, shortNames);
        Options.Add(opt);

        return opt;
    }

    public EnumOption<T> AddEnumOption<T>(char shortName) where T : struct, Enum => AddEnumOption<T>(new string[] { }, new[] { shortName });
    public EnumOption<T> AddEnumOption<T>(char[] shortNames) where T : struct, Enum => AddEnumOption<T>(new string[] { }, shortNames);
    public EnumOption<T> AddEnumOption<T>(string name) where T : struct, Enum => AddEnumOption<T>(new[] { name }, new char[] { });
    public EnumOption<T> AddEnumOption<T>(string[] names) where T : struct, Enum => AddEnumOption<T>(names, new char[] { });
    public EnumOption<T> AddEnumOption<T>(string name, char shortName) where T : struct, Enum => AddEnumOption<T>(new[] { name }, new[] { shortName });
    public EnumOption<T> AddEnumOption<T>(string[] names, char shortName) where T : struct, Enum => AddEnumOption<T>(names, new[] { shortName });
    public EnumOption<T> AddEnumOption<T>(string name, char[] shortNames) where T : struct, Enum => AddEnumOption<T>(new[] { name }, shortNames);
    public EnumOption<T> AddEnumOption<T>(string[] names, char[] shortNames) where T : struct, Enum
    {
        var opt = new EnumOption<T>(names, shortNames);
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
                ['-', char shortName] => ParseShortNameOption(shortName),
                ['-', '-', .. ReadOnlySpan<char> name] => ParseNameOption(name.ToString()),
                ReadOnlySpan<char> arg => ParseArgument(arg.ToString()),
            };

            if (parseResult is null)
            {
                throw new CommandLineException($"Unhandled error processing argument: {nextToken}");
            }
            
            OptionBase ParseShortNameOption(char shortName)
            {
                foreach (var opt in Options)
                {
                    if (opt.ShortNames.Contains(shortName))
                    {
                        return ParseOption(opt);
                    }
                }

                throw new CommandLineException($"Invalid option: {shortName}");
            }

            OptionBase ParseNameOption(string name)
            {
                foreach (var opt in Options)
                {
                    if (opt.Names.Contains(name))
                    {
                        return ParseOption(opt);
                    }
                }

                throw new CommandLineException($"Invalid option: {name}");
            }

            OptionBase ParseOption(OptionBase opt)
            {
                if (opt is Option boolOption)
                {
                    boolOption.IsPresent();
                }
                else if (opt is ParsedOption parsedOption)
                {
                    parsedOption.Parse(args[argIndex++]);
                }
                else
                {
                    Debug.Assert(false, "This should be unreachable");
                }

                return opt;
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
