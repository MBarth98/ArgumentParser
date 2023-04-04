namespace Examples;

using Argument;
using Argument.Context;
using Argument.Type;

public class Program
{
    public static void Main(string[] args)
    {
        var exec = new Executor();
        
        exec.AddHandler(new Property("test", "BOOLEAN", VALUE_TYPE_ENUM.BOOLEAN), (context) => {
            Console.WriteLine($"test={context.value}");
        });

        var cmd = new Scanner(args, exec);
        cmd.ParseAndCommit();
    }
}