using System.Reflection.Emit;

namespace Argument;
using Argument.Type;

using System.Net;

public class Validator
{
    public Validator()
    {
        this.Set(VALUE_TYPE_ENUM.STRING, (s) => true);
        this.Set(VALUE_TYPE_ENUM.CSV, (s) => true);
        this.Set(VALUE_TYPE_ENUM.INTEGER, (s) => int.TryParse(s, out _));
        this.Set(VALUE_TYPE_ENUM.FLOAT, (s) => float.TryParse(s, out _));
        this.Set(VALUE_TYPE_ENUM.BOOLEAN, (s) => bool.TryParse(s, out _));
        this.Set(VALUE_TYPE_ENUM.IP, (s) => IPAddress.TryParse(s, out _));
        this.Set(VALUE_TYPE_ENUM.URL, (s) => Uri.TryCreate(s, UriKind.Absolute, out _));
        this.Set(VALUE_TYPE_ENUM.FILE, (s) => System.IO.File.Exists(s));
        this.Set(VALUE_TYPE_ENUM.DIRECTORY, (s) => System.IO.Directory.Exists(s));
        this.Set(VALUE_TYPE_ENUM.ENUMERATION, (actual, valid) => valid.Contains(actual.ToUpper()));
    }

    public bool Validate(VALUE_TYPE_ENUM type, params object[] args)
    {
        return this.validators[type](args);
    }

    internal void Set(VALUE_TYPE_ENUM type, System.Func<string, string[], bool> validator)
    {
        this.validators[type] = (s) => {          
            return validator(s[0] as string ?? "", s[1] as string[] ?? new string[0] );
        };
    }

    internal void Set(VALUE_TYPE_ENUM type, System.Func<string, bool> validator)
    {
        if (type == VALUE_TYPE_ENUM.ENUMERATION)
        {
            throw new Exception("Use Set(VALUE_TYPE, Func<string, bool>) instead", new ArgumentException(nameof(validator)));
        }

        this.validators[type] = (s) => { 
            if (s.Length != 1 ||s[0] is not string @string)
            {
                throw new Exception("Incorrect types", new ArgumentException(nameof(validator)));
            }

            return validator(@string);
        };
    }

    private delegate bool ValidatorDelegate(params object[] args);
    private Dictionary<VALUE_TYPE_ENUM,  ValidatorDelegate> validators = new();
}