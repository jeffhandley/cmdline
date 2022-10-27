using System.Diagnostics.CodeAnalysis;

namespace System.CommandLine;

public abstract class OptionBase
{
    public string? Name { get; set; }
    public char? ShortName { get; set; }

    internal OptionBase(string name, char? shortName)
    {
        Name = name;
        ShortName = shortName;
    }

    public abstract void Parse(string arg);
}

public class Option : OptionBase
{
    public string Value { get; set; } = string.Empty;

    public Option(string name, char? shortName = null) : base(name, shortName) { }

    public override void Parse(string arg)
    {
        Value = arg;
    }
}

public class Option<T> : OptionBase where T : IParsable<T>
{
    public Option(string name, char? shortName) : base(name, shortName) { }

    public T? Value { get; set; }

    [MemberNotNull("Value")]
    public override void Parse(string arg)
    {
        Value = T.Parse(arg, null);
    }
}

public class EnumOption<T> : OptionBase where T : struct, Enum
{
    public T? Value { get; set; }

    public EnumOption(string name, char? shortName) : base(name, shortName) { }

    public override void Parse(string arg)
    {
        Value = Enum.Parse<T>(arg, true);
    }
}