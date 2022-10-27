using System.Diagnostics.CodeAnalysis;

namespace System.CommandLine;

public abstract class ArgumentBase
{
    public string[] Names { get; set; }
    public char[] ShortNames { get; set; }

    internal ArgumentBase(string[] names, char[] shortNames)
    {
        Names = names;
        ShortNames = shortNames;
    }

    public abstract void Parse(string arg);
}

public class Argument : ArgumentBase
{
    private string? _value;
    public Argument(string[] names, char[] shortNames) : base(names, shortNames) { }

    public string Value
    {
        get
        {
            if (_value is null)
            {
                throw new InvalidOperationException("Cannot get Value until Parse is called");
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
    public Argument(string[] names, char[] shortNames) : base(names, shortNames) { }

    public T Value
    {
        get
        {
            if (_value is null)
            {
                throw new InvalidOperationException("Cannot get Value until Parse is called");
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
