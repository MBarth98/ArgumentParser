namespace ArgumentParser.Error.Validator;

using System.Net;

public interface IValidators
{
    bool ValidateString(string? @string);
    bool ValidateCsv(string? @string);
    bool ValidateInteger(string? @string);
    bool ValidateEnumeration(string? @string, string[] valid);
    bool ValidateFile(string? @string);
    bool ValidateDirectory(string? @string);
    bool ValidateUrl(string? @string);
    bool ValidateIp(string? @string);
    bool ValidateBoolean(string? @string);
    bool ValidateFloat(string? @string);
}