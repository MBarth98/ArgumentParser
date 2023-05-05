namespace ArgumentParser.Context;

public sealed class PropertyContext : Context
{
    public PropertyContext(Context context, string key, string value) 
    : base(ref context)
    {
        this.key = key;
        this.value = value;
    }

    public readonly string key;
    public readonly string value;
}
