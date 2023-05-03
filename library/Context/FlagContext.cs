namespace ArgumentParser.Context;

using ArgumentParser.Type;

public class FlagContext : DefaultContext
{
    public FlagContext(DefaultContext context) 
    : base(ref context) { }

}
