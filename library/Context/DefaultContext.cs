namespace Argument.Context;

public class DefaultContext
{
    public DefaultContext(int index, string text)
    {
        this.index = index;
        this.text = text;
    }

    /// <summary>
    /// copy constructor
    /// </summary>
    /// <param name="context">another instance</param>
    public DefaultContext(ref DefaultContext context)
    {
        this.index = context.index;
        this.text = context.text;
    }

    public readonly int index;
    public readonly string text;
}

internal record DefaultHandler(Action<DefaultContext> callback);
