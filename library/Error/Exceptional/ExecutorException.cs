namespace ArgumentParser.Error.Exceptional;

using Argument.Type;

public class ExecutorException : ArgumentBaseException
{
    public ExecutorException() : base() { }
}


public class ValidatorException : ExecutorException
{
    public ValidatorException(string fieldName, VALUE_TYPE_ENUM setter)
    {

    }
}