namespace Argument.Context;

internal class HandlerList<Handler, Context> : List<(Handler handler, List<Context> contexts)> 
where Handler : class
where Context : DefaultContext 
{
    public HandlerList<Handler, Context> Add(Handler handler)
    {
        this.Add((handler, new()));
        return this;
    }

    public HandlerList<Handler, Context> Add(Handler? handler, Context context) 
    {
        if (handler is null)
        {
            this[0].contexts.Add(context);
            return this;
        }

        this.Where((next) => next.handler.Equals(handler)).SingleOrDefault().contexts.Add(context);
        return this;
    }
}

class FlagHandlerList : HandlerList<FlagHandler, FlagContext> { }
class PropertyHandlerList : HandlerList<PropertyHandler, PropertyContext> { }
class DefaultHandlerList : HandlerList<DefaultHandler, DefaultContext> { }
