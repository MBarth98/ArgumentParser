namespace ArgumentParser.Type;


public enum VALUE_TYPE_ENUM
{
    /// <summary>
    /// default (raw input)
    /// </summary>
    STRING,

    /// <summary>
    /// comma seperated values
    /// </summary>
    CSV,

    /// <summary>
    /// integer
    /// </summary>
    INTEGER,

    /// <summary>
    /// number with decimal point, seperated by a dot (.)
    /// </summary>
    FLOAT,

    /// <summary>
    /// true/false
    /// </summary>
    BOOLEAN,

    /// <summary>
    /// ipv4 address (xxx.xxx.xxx.xxx)
    /// </summary>
    IP,

    /// <summary>
    /// url scheme>:
    /// </summary>
    URL,

    /// <summary>
    /// file path
    /// </summary>
    FILE,

    /// <summary>
    /// directory path
    /// </summary>
    DIRECTORY,

    /// <summary>
    /// enum (C# enum as string)
    /// </summary>
    ENUMERATION
}

