using ArgumentParser.Type;

namespace ArgumentParser.Context;

public sealed class PropertyContext : BaseContext
{
    public PropertyContext(string text, Option option, string key, string value)
    : base(text, option)
    {
        this.key = key;
        this.value = value;
    }
    
    public PropertyContext(BaseContext context, string key, string value) 
    : base(ref context)
    {
        this.key = key;
        this.value = value;
    }
    
    public readonly string key;
    public readonly string value;
}
