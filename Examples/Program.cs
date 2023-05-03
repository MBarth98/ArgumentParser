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

             act_help        = new Action(help_fn, { "-h", "--help", "?" }, "help", ASSOC.LEFT)
             act_remove_all  = new Action(remove_all_fn, {"-a", "--all"}, "all", ASSOC.LEFT)

                * default assoc is ASSOC.BOTH

             act_verbose     = new Action(set_verbose_fn, {"-v", "--verbose"}, "verbose", ASSOC.BOTH)
             act_remove      = new Property(remove_fn, {"remove", "rm"}, "remove")
             act_install     = new Property(install_fn, {"install", "i"}, "install")

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

             cmd.register(
                act_help,
                act_remove_all,
                act_verbose,
                act_remove,
                act_install
             );

             cmd.Paths = {
                "" => act_help,
                "install" => act_install,
             }

             cmd.Arguments = {
                 new Flag(help_fn, "-h", "help"),
                 new Property(install_fn, "install"),
                 new Property(remove_fn, "remove"),
                 new Scope("parameter") {
                     remove_all,
                     new Flag(help_fn, "-h", "help"),
                 }
             }

            //----------------------------------------------------------------------------------------------------
        */        
        
        var arguments = new Scanner(args);

        var install_fn      = delegate (PropertyContext context) { Console.WriteLine($"install {context.value}"); };
        var remove_fn       = delegate (PropertyContext context) { Console.WriteLine($"remove {context.value}"); };
        var remove_all_fn   = delegate (FlagContext context) { Console.WriteLine($"remove all"); };
        var help_fn         = delegate (FlagContext context) { Console.WriteLine($"help"); };
        var set_verbose_fn  = delegate (FlagContext context) { Console.WriteLine($"set verbose"); };

        var tok_install     = new Token(install_fn, "install", 0, "i");
        var tok_remove      = new Token(remove_fn, "remove", 0, "rm");
        var tok_remove_all  = new Token(remove_all_fn, "all", 1, "all");
        var tok_verbose     = new Token(set_verbose_fn, "verbose",2, "-v");
        var tok_help        = new Token(help_fn, "help", 3, "-h", "?");

        var root = new Group();

        root.Add(tok_help);

        var install = new Group(tok_install);
        install.Add(tok_verbose);
        install.Add(tok_help);

        var remove = new Group(tok_remove);
        remove.Add(tok_remove_all);
        install.Add(tok_verbose);
        remove.Add(tok_help);

        root.Add(install);
        root.Add(remove);

        arguments.CallHandlers();
    }

    class Token
    {
        public string name;
        public int assosiativity;
        public List<string> selectors;
        public Action<dynamic> action;

        private Token(Action<dynamic> action, string name, int assosiativity, params string[] selectors) 
        {
            this.name = name;
            this.assosiativity = assosiativity;
            this.selectors = new List<string>(selectors);
            this.action = action;
        }

        public Token(Action<PropertyContext> action, string name, int assosiativity, params string[] selectors)
        : this((Action<dynamic>)action, name, assosiativity, selectors) {}

        public Token(Action<FlagContext> action, string name, int assosiativity, params string[] selectors)
        : this((Action<dynamic>)action, name, assosiativity, selectors) {}
    }

    class Group
    {
        public readonly string name = "";
        public readonly List<Group> SubGroups = new();
        public readonly List<Token> Tokens = new();


        public Group Add(Group child)
        {
            this.SubGroups.Add(child);
            return child;
        }

        public Group Add(Token token)
        {
            this.Tokens.Add(token);
            return this;
        }

        public Group(Token token)
        {
            this.name = token.name;
            this.Tokens.Add(token);
        }

        public Group()
        {
            this.name = "";
        }
    }
}