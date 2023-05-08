namespace ArgumentParser.Error.Exceptional;

public class ArgumentBaseException : Exception
{
    public ArgumentBaseException(string msg) : base(msg) { }
}