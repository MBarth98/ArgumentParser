using ArgumentParser.Type;

namespace ArgumentParser.Context;

public sealed class ActionContext : BaseContext
{
    public ActionContext(string text, Option option)
    : base(text, option) { }
    
    public ActionContext(BaseContext context) 
    : base(ref context) { }

}
