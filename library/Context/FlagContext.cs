namespace Argument.Context;

using Argument.Type;

public class FlagContext : DefaultContext
{
    public FlagContext(DefaultContext context) : base(ref context)
    {
    }

}

internal record FlagHandler(Flag flag, Action<FlagContext> callback);
