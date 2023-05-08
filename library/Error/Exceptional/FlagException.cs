namespace ArgumentParser.Error.Exceptional;

public class FlagException : ParserException
{
    public FlagException(string msg) : base(msg)
    {
    }
}