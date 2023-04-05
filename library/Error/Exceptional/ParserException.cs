namespace ArgumentParser.Error.Exceptional;

public class ParserException : ArgumentBaseException
{
    public ParserException(string msg) : base(msg)
    {
    }
}

