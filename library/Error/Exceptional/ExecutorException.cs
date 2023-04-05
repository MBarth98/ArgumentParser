namespace ArgumentParser.Error.Exceptional;

using ArgumentParser.Type;
using System.Runtime.CompilerServices;

public class ExecutorException : ArgumentBaseException
{
    public ExecutorException(string msg) : base(msg) { }

}
