using System.Diagnostics.CodeAnalysis;

namespace System.CommandLine;

public abstract class ArgumentBase
{
    public string? Name { get; set; }
    public char? ShortName { get; set; }

    internal ArgumentBase(string? name, char? shortName)
    {
        Name = name;
        ShortName = shortName;
    }

    public abstract void Parse(string arg);
}

public class Argument : ArgumentBase
{
    private string? _value;
    public Argument(string? name, char? shortName) : base(name, shortName) { }

    public string Value
    {
        get
        {
            if (_value is null)
            {
                throw new InvalidOperationException("Cannot read Value until Parse is called");
            }

            return _value;
        }
    }

    [MemberNotNull(nameof(_value))]
    public override void Parse(string arg)
    {
        _value = arg;
    }
}

public class Argument<T> : ArgumentBase where T : IParsable<T>
{
    private T? _value;
    public Argument(string? name, char? shortName) : base(name, shortName) { }

    public T Value
    {
        get
        {
            if (_value is null)
            {
                throw new InvalidOperationException("Cannot read Value until Parse is called");
            }

            return _value;
        }
    }

    [MemberNotNull(nameof(_value))]
    public override void Parse(string arg)
    {
        _value = T.Parse(arg, null);
    }
}
