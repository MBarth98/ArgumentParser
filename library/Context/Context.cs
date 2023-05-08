using ArgumentParser.Error.Exceptional;
using ArgumentParser.Type;

namespace ArgumentParser.Context;

public abstract class BaseContext
{
    private static int m_index = 0;

    public BaseContext(string text, Option option)
    {
        this.Option = option;
        Index = m_index++;
        Text = text;
    }

    /// <summary>
    /// copy constructor
    /// </summary>
    /// <param name="context">another instance</param>
    public BaseContext(ref BaseContext context)
    {
        Option = context.Option;
        Index = context.Index;
        Text = context.Text;
    }

    public readonly Option Option;

    /// <summary>
    /// the index of the argument relative to other parsed arguments
    /// </summary>
    public readonly int Index;

    /// <summary>
    /// the original text of the argument
    /// </summary>
    public readonly string Text;

    /// <summary>
    /// the index of the first character of the argument in the original string
    /// </summary>
    public readonly int StartIndex = 0;

    /// <summary>
    /// the index of the last character of the argument in the original string
    /// </summary>
    public readonly int EndIndex = 0;

    /// <summary>
    /// null if no error occurred for this context during parsing
    /// </summary>
    public readonly ParserException? Error = null;
}

