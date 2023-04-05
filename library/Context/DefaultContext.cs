using ArgumentParser.Error.Exceptional;

namespace Argument.Context;

public class DefaultContext
{
    public DefaultContext(int index, string text)
    {
        this.Index = index;
        this.Text = text;
    }

    /// <summary>
    /// copy constructor
    /// </summary>
    /// <param name="context">another instance</param>
    public DefaultContext(ref DefaultContext context)
    {
        this.Index = context.Index;
        this.Text = context.Text;
    }

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

