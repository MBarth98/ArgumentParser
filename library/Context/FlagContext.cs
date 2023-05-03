namespace ArgumentParser.Context;

using ArgumentParser.Type;

public class ActionContext : DefaultContext
{
    public ActionContext(DefaultContext context) 
    : base(ref context) { }

}
