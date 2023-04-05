namespace ArgumentParser.Context;

using Type;

public class PropertyContext : DefaultContext
{
    public PropertyContext(DefaultContext context, string key, string value) : base(ref context)
    {
        this.key = key;
        this.value = value;
    }

    public readonly string key;
    public readonly string value;
}

internal record PropertyHandler(Property data, Action<PropertyContext> callback);
