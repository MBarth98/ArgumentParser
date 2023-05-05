using System;
using System.Collections.Generic;
using ArgumentParser;
using ArgumentParser.Context;
using ArgumentParser.Type;

namespace Examples;

public class Program
{


    public static void Main(string[] args)
    {
        /*
            //---------------------------------------------------------------------------------------------------- 

             fn install_fn (context) => State {
                 if call install with context.Value then State.STOP else State.FATAL_ERROR;
             }

             fn remove_fn (context) => State {
                 if call remove with context.Value then State.STOP else State.FATAL_ERROR;
             }

             fn remove_all_fn (context) => State {
                 foreach p in programs if call p.remove then yield State.STOP else State.FATAL_ERROR;
             }

             fn help_fn (context) => State {
                 print get_help with context.path then State.STOP 
             }

             fn set_verbose_fn (context) => State {
                 set stateholder.verbose true then State.CONTINUE
             }

            //----------------------------------------------------------------------------------------------------

             Property:
                 keyvalue pair with a customizable value type and delimiter
                 - 'install <package>'
                 - 'remove <package>'

              Action:
                Single identifier that calls a function when matched
                 - '-v'
                 - '-h'

            //---------------------------------------------------------------------------------------------------- 

             help                    // act_help
             install                 // act_install 
             install.help            // act_help
             install.verbose         // act_verbose => act_install
             install.verbose.help    // act_verbose => act_help
             remove                  // act_remove
             remove.help             // act_help
             remove.all              // act_remove_all
             remove.all.help         // act_help

            //---------------------------------------------------------------------------------------------------- 

             ?> program.exe remove all
                 => "remove.all" => call remove_fn => remove all of the things

             ?> program.exe remove -h
                 => "remove.help" => call help_fn => show help for remove

             ?> program.exe -h
                 => "help" => call help_fn => show help for program

             ?> program.exe install <package> -v
                 => "install.verbose" => call set_verbose_fn => call install_fn

             ?> program.exe 
                 => "" => call default => call help_fn => show help for program 

            //----------------------------------------------------------------------------------------------------
        */

        static CallbackState install_fn(PropertyContext context)
        {
            Console.WriteLine($"install {context.value}");
            return CallbackState.STOP;
        }

        static CallbackState remove_fn(PropertyContext context)
        {
            Console.WriteLine($"remove {context.value}");
            return CallbackState.STOP;
        }

        static CallbackState remove_all_fn(ActionContext context)
        {
            Console.WriteLine($"remove all");
            return CallbackState.STOP;
        }

        static CallbackState help_fn(ActionContext context)
        {
            Console.WriteLine($"help");
            return CallbackState.STOP;
        }

        static CallbackState set_verbose_fn(ActionContext context)
        {
            Console.WriteLine($"set verbose");
            return CallbackState.CONTINUE;
        }

        var tok_install     = new Token(install_fn, new PropertyValue("install", 0, "i", "install"));
        var tok_remove      = new Token(remove_fn, new PropertyValue("remove", 0, "rm", "remove"));
        var tok_remove_all  = new Token(remove_all_fn, new ActionValue("all", 1, "all"));
        var tok_verbose     = new Token(set_verbose_fn, new ActionValue("verbose",2, "-v"));
        var tok_help        = new Token(help_fn, new ActionValue("help", 3, "-h", "?"));

        var root = new Group();

        root.Add(tok_help);

        var install = new Group(tok_install);
        install.Add(tok_verbose);
        install.Add(tok_help);

        var remove = new Group(tok_remove);
        remove.Add(tok_remove_all);
        remove.Add(tok_verbose);
        remove.Add(tok_help);

        root.Add(install);
        root.Add(remove);

        Console.WriteLine(root.ToDot());

        var arguments = new Scanner(args, root);
        
        arguments.CallHandlers();
    }

}