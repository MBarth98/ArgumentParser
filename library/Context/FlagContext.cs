namespace Argument.Context;

using Argument.Type;

public class FlagContext : DefaultContext
{
    public FlagContext(DefaultContext context, string name) : base(ref context)
    {
        this.name = name;
    }

    public readonly string name;
}

internal record FlagHandler(Flag flag, Action<FlagContext> callback);
