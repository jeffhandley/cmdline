namespace System.CommandLine;

public class CommandLineException : ArgumentException
{
    public CommandLineException(string error) : base(error) { }
}
