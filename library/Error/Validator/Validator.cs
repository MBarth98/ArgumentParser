
namespace Argument.Error;

using ArgumentParser.Error.Validator;
using Type;

public class Validator
{
    public Validator(IValidators impl)
    {
        Set(VALUE_TYPE_ENUM.STRING, impl.ValidateString);
        Set(VALUE_TYPE_ENUM.CSV, impl.ValidateCsv);
        Set(VALUE_TYPE_ENUM.INTEGER, impl.ValidateInteger);
        Set(VALUE_TYPE_ENUM.FLOAT, impl.ValidateFloat);
        Set(VALUE_TYPE_ENUM.BOOLEAN, impl.ValidateBoolean);
        Set(VALUE_TYPE_ENUM.IP, impl.ValidateIp);
        Set(VALUE_TYPE_ENUM.URL, impl.ValidateUrl);
        Set(VALUE_TYPE_ENUM.FILE, impl.ValidateFile);
        Set(VALUE_TYPE_ENUM.DIRECTORY, impl.ValidateDirectory);
        Set(VALUE_TYPE_ENUM.ENUMERATION, impl.ValidateEnumeration);
    }

    public bool Validate(VALUE_TYPE_ENUM type, params object[] args)
    {
        return validators[type](args);
    }

    internal void Set(VALUE_TYPE_ENUM type, Func<string, string[], bool> validator)
    {
        validators[type] = (s) => 
            validator(s[0] as string ?? "", s[1] as string[] ?? Array.Empty<string>() );
    }

    internal void Set(VALUE_TYPE_ENUM type, Func<string, bool> validator)
    {
        if (type == VALUE_TYPE_ENUM.ENUMERATION)
        {
            throw new Exception("Use Set(VALUE_TYPE, Func<string, bool>) instead", 
                new ArgumentException(nameof(validator)));
        }

        validators[type] = (s) => { 
            if (s.Length != 1 ||s[0] is not string @string)
            {
                throw new Exception("Incorrect types", 
                    new ArgumentException(nameof(validator)));
            }

            return validator(@string);
        };
    }

    private delegate bool ValidatorDelegate(params object[] args);
    private Dictionary<VALUE_TYPE_ENUM,  ValidatorDelegate> validators = new();

}