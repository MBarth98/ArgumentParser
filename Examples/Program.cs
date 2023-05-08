using ArgumentParser;
using ArgumentParser.Context;
using ArgumentParser.Type;

namespace Examples;

public class Program
{
    public static void Main(string[] args)
    {
        static CallbackState install_fn(PropertyContext context)
        {
            Console.WriteLine($"install_fn {context.value}");
            return CallbackState.STOP;
        }

        static CallbackState remove_fn(PropertyContext context)
        {
            Console.WriteLine($"remove_fn {context.value}");
            return CallbackState.STOP;
        }

        static CallbackState remove_all_fn(ActionContext context)
        {
            Console.WriteLine($"remove_all_fn");
            return CallbackState.STOP;
        }

        static CallbackState help_fn(ActionContext context)
        {
            Console.WriteLine($"help_fn");
            return CallbackState.STOP;
        }

        static CallbackState set_verbose_fn(ActionContext context)
        {
            Console.WriteLine($"set_verbose_fn");
            return CallbackState.CONTINUE;
        }

        var tok_install     = new Option(install_fn, new PropertyValue("install", 0, "i", "install"));
        var tok_remove      = new Option(remove_fn, new PropertyValue("remove", 0, "rm", "remove"));
        var tok_remove_all  = new Option(remove_all_fn, new ActionValue("all", 1, "all"));
        var tok_verbose     = new Option(set_verbose_fn, new ActionValue("verbose",2, "-v"));
        var tok_help        = new Option(help_fn, new ActionValue("help", 3, "-h"));

        var options = new Options();

        options.Add(tok_help);

        var install_options = new Options(tok_install);
        install_options.Add(tok_verbose);
        install_options.Add(tok_help);

        var remove_options = new Options(tok_remove);
        remove_options.Add(tok_remove_all);
        remove_options.Add(tok_verbose);
        remove_options.Add(tok_help);

        options.Add(install_options);
        options.Add(remove_options);

        //Console.WriteLine(root.ToDot());
        //Console.WriteLine();
        //Console.WriteLine(root.ToPath());


        var arguments = new Scanner(args, options);
        
        arguments.CallHandlers();
    }

}