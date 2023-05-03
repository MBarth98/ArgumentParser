using ArgumentParser.Error.Validator;

namespace ArgumentParser.Type;

public class Property : Identifier
{
    public Property(SelectorSet selectors, string name)
    {
        this.m_property_name = name;
        this.m_selectors = selectors;
    }

    /// <summary>
    ///  set the description of the option
    /// </summary>
    /// <param name="description"></param>
    public void Description(string description)
    {
        this.m_description = description;
    }

    /// <summary>
    /// make the key required (at least one instance of the key is required)
    /// </summary>
    /// <param name="isRequired">default: false</param>
    public void Required(bool isRequired = false)
    {
        this.m_isRequired = isRequired;
    }

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
    /// allow multiple instances of the same key (default: false)
    /// </summary>
    /// <param name="allowMultiple">default: false</param>
    public void AllowMultiple(bool allowMultiple = false)
    {
        this.m_allowMultiple = allowMultiple;
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

    public void AddSelector(string selector)
    {
        this.m_selectors.Add(selector);
    }

    public void RemoveSelector(string selector)
    {
        this.m_selectors.Remove(selector);
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

    internal bool IsRequired() => this.m_isRequired;
    internal bool IsCaseSensitive() => this.m_isCaseSensitive;

    internal bool AllowEmpty() => this.m_allowEmpty;
    internal bool AllowWhiteSpace() => this.m_allowWhiteSpace;
    internal bool AllowMultiple() => this.m_allowMultiple;
    internal VALUE_TYPE_ENUM ValueType() => this.m_property_type;
    internal System.Type EnumType() => this.m_property_value_enum;
    internal string Key() => this.m_property_name;
    internal string Name() => this.m_property_name;
    internal string Description() => this.m_description;
    internal string? Delimiter() => this.m_delimiter;
    internal string? Value() => this.m_property_value;
    internal string[] Selectors() => this.m_selectors.ToArray();

    /// <summary>
    /// mark the key as required (at least one instance of the key is required)
    /// </summary>
    private bool m_isRequired = false;

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
    /// allow multiple instances of the same key
    /// </summary>
    private bool m_allowMultiple;

    /// <summary>
    /// expected data type of the value (default: string)
    /// </summary>
    private VALUE_TYPE_ENUM m_property_type;

    /// <summary>
    /// if valueType is set to ENUM, this must be the enum type
    /// </summary>
    private System.Type m_property_value_enum = null!;

    /// <summary>
    /// unique name of the option
    /// </summary>
    private readonly string m_property_name;

    /// <summary>
    /// description of the option
    /// </summary>
    private string m_description = "";

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

    private readonly SelectorSet m_selectors = new();

    private readonly ValidatorImpl m_validator = new(new DefaultValidators());

}