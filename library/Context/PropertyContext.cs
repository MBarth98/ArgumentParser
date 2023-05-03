namespace ArgumentParser.Context;

public class PropertyContext : DefaultContext
{
    public PropertyContext(DefaultContext context, string key, string value) 
    : base(ref context)
    {
        this.key = key;
        this.value = value;
    }

    public readonly string key;
    public readonly string value;
}
