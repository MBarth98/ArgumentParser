namespace ArgumentParser.Error.Validator;

using System.Net;

public class DefaultValidators : IValidators
{
    public bool ValidateString(string? @string)
    {
        return true;
    }

    public bool ValidateCsv(string? @string)
    {
        return true;
    }

    public bool ValidateInteger(string? @string)
    {
        return int.TryParse(@string, out _);
    }

    public bool ValidateEnumeration(string? @string, string[] valid)
    {
        return valid.Contains(@string?.ToUpper() ?? "");
    }

    public bool ValidateFile(string? @string)
    {
        return File.Exists(@string);
    }

    public bool ValidateDirectory(string? @string)
    {
        return Directory.Exists(@string);
    }

    public bool ValidateUrl(string? @string)
    {
        return Uri.TryCreate(@string, UriKind.Absolute, out _);
    }

    public bool ValidateIp(string? @string)
    {
        return IPAddress.TryParse(@string, out _);
    }

    public bool ValidateBoolean(string? @string)
    {
        return bool.TryParse(@string, out _);
    }

    public bool ValidateFloat(string? @string)
    {
        return float.TryParse(@string, out _);
    }
}