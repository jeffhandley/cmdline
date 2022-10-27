using System.Diagnostics.CodeAnalysis;

namespace System.CommandLine;

public abstract class ArgumentBase
{
    public string Name { get; set; }
    public char? ShortName { get; set; }

    internal ArgumentBase(string name, char? shortName)
    {
        Name = name;
        ShortName = shortName;
    }

    public abstract void Parse(string arg);
}

public class Argument : ArgumentBase
{
    public string Value { get; set; } = string.Empty;

    public Argument(string name, char? shortName) : base(name, shortName) { }

    public override void Parse(string arg)
    {
        Value = arg;
    }
}

public class Argument<T> : ArgumentBase where T : IParsable<T>
{
    public Argument(string name, char? shortName) : base(name, shortName) { }

    public T? Value { get; set; }

    [MemberNotNull("Value")]
    public override void Parse(string arg)
    {
        Value = T.Parse(arg, null);
    }
}
