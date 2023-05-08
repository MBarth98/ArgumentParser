namespace ArgumentParser.Error.Exceptional;

public class PropertyException : ParserException
{
    public PropertyException(string msg) : base(msg)
    {
    }
}


public class EnumPropertyException : PropertyException
{
    public EnumPropertyException(string msg) : base(msg)
    {
    }
}

