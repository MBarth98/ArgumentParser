namespace Examples;

using Argument;
using Argument.Context;
using Argument.Type;

public class Program
{
    public static void Main(string[] args)
    {
        var cmd = new Parser(args);
        cmd.AddHandler(new Property("test", "BOOLEAN", VALUE_TYPE_ENUM.BOOLEAN), (context) => {
            Console.WriteLine($"test={context.value}");
        });
        cmd.Process();
    }
}