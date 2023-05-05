using System.Diagnostics;
using ArgumentParser.Error.Validator;

namespace ArgumentParser.Type;

public sealed class PropertyValue : Parameter
{
    public PropertyValue(string name, int precedence, params string[] selectors)
    : base(name, precedence, selectors) { }

    /// <summary>
    /// make the key and value case sensitive (default: false)
    /// </summary>
    /// <param name="isCaseSensitive">default: false</param>
    public void CaseSensitive(bool isCaseSensitive = false)
    {
        this.m_isCaseSensitive = isCaseSensitive;
    }

    /// <summary>
    /// make the value optional (default: false)
    /// </summary>
    /// <param name="allowEmpty">default: false</param>
    public void AllowEmpty(bool allowEmpty = false)
    {
        this.m_allowEmpty = allowEmpty;
    }

    /// <summary>
    /// allow whitespace in the value (default: true)
    /// </summary>
    /// <param name="allowWhiteSpace">default: true</param>
    public void AllowWhiteSpace(bool allowWhiteSpace = true)
    {
        this.m_allowWhiteSpace = allowWhiteSpace;
    }

    /// <summary>
    /// set the expected data type of the value (default: string)
    /// </summary>
    /// <param name="valueType">default: string</param>
    public void ValueType(VALUE_TYPE_ENUM valueType)
    {
        if (valueType == VALUE_TYPE_ENUM.ENUMERATION)
        {
            throw new ArgumentException("Use SetValueType(Enum @enum) instead", nameof(valueType));
        }

        this.m_property_type = valueType;
    }

    /// <summary>
    /// set the expected data type of the value to enum
    /// </summary>
    /// <param name="enum">the enum type</param>
    /// <param name="type">constant: enum</param>
    public void ValueType(System.Type @enum)
    {
        this.m_property_type = VALUE_TYPE_ENUM.ENUMERATION;
        this.m_property_value_enum = (dynamic) @enum;
    }

    public void Validator(Func<string, bool> validator)
    {
        this.m_validator.Set(this.m_property_type, validator);
    }

    public void Validator(Func<string, string[], bool> validator)
    {
        this.m_validator.Set(this.m_property_type, validator);
    }

    public void Default(string @default)
    {
        this.Value(@default);
    }


    internal void Value(string value)
    {
        object[] args;
        if (this.m_property_type == VALUE_TYPE_ENUM.ENUMERATION)
        {
            if (EnumType() == null)
            {
                throw new InvalidOperationException("EnumType is null");
            }

            args = new object[] { value , EnumType().GetEnumNames() };
        } 
        else
        {
            args = new object[] { value };
        }

        if (!this.m_validator.Validate(this.m_property_type, args))
        {
            throw new ArgumentException("Could not use value as an enum", nameof(value));
        }

        this.m_property_value = value;
    }

    internal bool IsCaseSensitive() => this.m_isCaseSensitive;
    internal bool AllowEmpty() => this.m_allowEmpty;
    internal bool AllowWhiteSpace() => this.m_allowWhiteSpace;
    internal VALUE_TYPE_ENUM ValueType() => this.m_property_type;
    internal System.Type EnumType() => this.m_property_value_enum;
    internal string Key() => base.Name();
    internal string? Delimiter() => this.m_delimiter;
    internal string? Value() => this.m_property_value;

    /// <summary>
    ///  
    /// </summary>
    private bool m_isCaseSensitive = false;

    /// <summary>
    /// allow empty value
    /// </summary>
    private bool m_allowEmpty = false;

    /// <summary>
    /// allow whitespace in the value
    /// </summary>
    private bool m_allowWhiteSpace = true;

    /// <summary>
    /// expected data type of the value (default: string)
    /// </summary>
    private VALUE_TYPE_ENUM m_property_type;

    /// <summary>
    /// if valueType is set to ENUM, this must be the enum type
    /// </summary>
    private System.Type m_property_value_enum = null!;

    /// <summary>
    /// delimiter between key and value (default: "=")
    /// </summary>
    /// <remarks>
    /// if null, the key must be followed by the value with any number of whitespace characters
    /// </remarks>
    private readonly string? m_delimiter = null;

    /// <summary>
    /// value of the key (if any)
    /// </summary>
    private string m_property_value = "";

    private readonly ValidatorImpl m_validator = new(new DefaultValidators());

}