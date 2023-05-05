namespace ArgumentParser.Context;

public sealed class ActionContext : Context
{
    public ActionContext(Context context) 
    : base(ref context) { }

}
