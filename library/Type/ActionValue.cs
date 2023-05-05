namespace ArgumentParser.Type;

public sealed class ActionValue : Parameter
{
    public ActionValue(string name, int precedence, params string[] selectors)
    : base(name, precedence, selectors) {}
}
