namespace ArgumentParser.Context;

internal class HandlerList<THandler, TContext> : List<(THandler handler, List<TContext> contexts)> 
where THandler : class
where TContext : DefaultContext 
{
    public HandlerList<THandler, TContext> Add(THandler handler)
    {
        this.Add((handler, new List<TContext>()));
        return this;
    }

    public HandlerList<THandler, TContext> Add(THandler? handler, TContext context) 
    {
        if (handler is null)
        {
            this[0].contexts.Add(context);
            return this;
        }

        this.SingleOrDefault(next => next.handler.Equals(handler)).contexts.Add(context);
        return this;
    }
}

internal class FlagHandlerList : HandlerList<FlagHandler, FlagContext> { }

internal class PropertyHandlerList : HandlerList<PropertyHandler, PropertyContext> { }
