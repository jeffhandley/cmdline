using System.Diagnostics.CodeAnalysis;

namespace System.CommandLine;

public abstract class OptionBase
{
    public string[] Names { get; set; }
    public char[] ShortNames { get; set; }

    internal OptionBase(string[] names, char[] shortNames)
    {
        Names = names;
        ShortNames = shortNames;
    }
}

public abstract class ParsedOption : OptionBase
{
    internal ParsedOption(string[] names, char[] shortNames) : base(names, shortNames) { }

    public abstract void Parse(string arg);
}

public class Option : OptionBase
{
    internal Option(string[] names, char[] shortNames) : base(names, shortNames) { }

    public bool Value { get; private set; }

    public void IsPresent()
    {
        Value = true;
    }
}

public class Option<T> : ParsedOption where T : IParsable<T>
{
    private T? _value;

    internal Option(string[] names, char[] shortNames) : base(names, shortNames) { }

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

public class EnumOption<T> : ParsedOption where T : struct, Enum
{
    public T? Value { get; set; }

    internal EnumOption(string[] names, char[] shortNames) : base(names, shortNames) { }

    public override void Parse(string arg)
    {
        Value = Enum.Parse<T>(arg, true);
    }
}