using ArgumentParser.Context;
using ArgumentParser.Type;
using ArgumentParser.Error.Exceptional;

namespace ArgumentParser;


public class Executor
{
    public void Commit()
    {
        if (m_hasRun)
        {
            throw new ExecutorException($"{nameof(Commit)} can only be called once.");
        }

        MergeHandlers();

        InvokeHandlers();

        m_hasRun = true;
    }
    private void InvokeHandlers()
    {
        // call handlers in order of discovery
        foreach (var handler in Handlers.OrderBy(next => next.Key))
        {
            handler.Value.Invoke();
        }

    }

    private void MergeHandlers()
    {
        foreach (var (handler, contexts) in FlagHandlers)
        {
            if (contexts.Count <= 0) continue;

            foreach (var context in contexts)
            {
                Handlers[context.Index] = () => handler.callback.Invoke(context);
            }
        }

        foreach (var (handler, contexts) in PropertyHandlers)
        {
            if (contexts.Count <= 0) continue;
            foreach (var keyValue in contexts)
            {
                Handlers[keyValue.Index] = () => handler.callback.Invoke(keyValue);
            }
        }

        if (DefaultHandler.Callback == null) return;

        foreach (var defaultContext in DefaultHandler.Contexts)
        {
            Handlers[defaultContext.Index] = () => DefaultHandler.Callback.Invoke(defaultContext);
        }
    }

    public void AddHandler(Flag flag, Action<FlagContext> handler)
    {
        var valid = FlagHandlers.All(next =>
        {
            foreach (var selector in flag.Selectors())
            {
                if (next.handler.flag.Selectors().Contains(selector))
                {
                    throw new ExecutorException($"{nameof(AddHandler)}(Flag {nameof(flag)}, Action<> {nameof(handler)}) : selector [{selector}] has already been bound to a callback");
                }

                m_validSelectors.Add(selector);
            }
            return true;
        });

        if (!valid) throw new ExecutorException($"{nameof(AddHandler)}(Flag {nameof(flag)}, Action<> {nameof(handler)}) : Unknown error");

        FlagHandlers.Add(new FlagHandler(flag, handler));
    }

    public void AddHandler(Property property, Action<PropertyContext> handler)
    {
        var thisKeys = property.Selectors().Append(property.Key());
        var valid = PropertyHandlers.All(next =>
        {
            var nextKeys = next.handler.data.Selectors().Append(next.handler.data.Key()).ToList();

            foreach (var thisKey in thisKeys)
            {
                if (nextKeys.Contains(thisKey))
                {
                    throw new ExecutorException($"{nameof(AddHandler)}(Property {nameof(property)}, Action<> {nameof(handler)}) : selector [{thisKey}] has already been bound to a callback");
                }

                m_validSelectors.Add(thisKey);
            }
            return true;
        });

        if (!valid) throw new ExecutorException($"{nameof(AddHandler)}(Property {nameof(property)}, Action<> {nameof(handler)}) : Unknown error");

        PropertyHandlers.Add(new PropertyHandler(property, handler));
    }

    public void AddHandler(object _, Action<DefaultContext> handler)
    {
        DefaultHandler = new DefaultCallback(handler, new List<DefaultContext>());
    }


    public List<string> GetFlagSelectors => 
        FlagHandlers.SelectMany(next => 
            next.handler.flag.Selectors()).ToList();

    public List<string> GetPropertySelectors => 
        PropertyHandlers.SelectMany(next => 
            next.handler.data.Selectors().Append(next.handler.data.Key())).ToList();


    internal void AddContext(FlagHandler handler, FlagContext context)
    {   
        FlagHandlers.Add(handler, context);
    }

    internal void AddContext(PropertyHandler handler, PropertyContext context)
    {
        PropertyHandlers.Add(handler, context);
    }

    internal void AddContext(string selector, FlagContext context)
    {
        if (!HasFlag(selector))
        {
            throw new ExecutorException($"{nameof(AddContext)} : Flag [{selector}] has not been registered, with a callback.");
        }
        
        FlagHandlers.Add(GetFlagHandler(selector), context);
    }

    internal void AddContext(string selector, PropertyContext context)
    {
        if (!HasProperty(selector))
        {
            throw new ExecutorException($"{nameof(AddContext)} : Property [{selector}] has not been registered, with a callback.");
        }
        
        PropertyHandlers.Add(GetPropertyHandler(selector), context);
    }

    internal void AddContext(DefaultContext context)
    {
        DefaultHandler.Contexts.Add(context);
    }

    internal bool HasDefaultHandler => DefaultHandler.Callback != null;
    internal bool HasFlagHandler => FlagHandlers.Count > 0;
    internal bool HasPropertyHandler => PropertyHandlers.Count > 0;


    internal bool HasAny(string selector)
    {
        try
        {
            return m_validSelectors.Contains(selector) || m_validSelectors.Any(selector.StartsWith);
        }
        catch { return false; }
    }

    internal bool IsExact(string selector)
    {
        return m_validSelectors.Contains(selector);
    }

    internal bool HasFlag(string selector)
    {
        return m_validSelectors.Contains(selector) &&
               FlagHandlers.Any(next => 
                   next.handler.flag.Selectors().Contains(selector));
    }

    internal bool HasProperty(string selector)
    {
        return m_validSelectors.Contains(selector) && 
               PropertyHandlers.Any(next => 
                   next.handler.data.Selectors().Contains(selector));
    }

    internal FlagHandler GetFlagHandler(string selector)
    {
        if (!HasFlag(selector))
        {
            throw new ExecutorException($"{nameof(GetFlagHandler)} : Flag [{selector}] has not been registered.");
        }
        return FlagHandlers.First(next => next.handler.flag.Selectors().Contains(selector)).handler;
    }

    internal PropertyHandler GetPropertyHandler(string selector)
    {
        if (!HasProperty(selector))
        {
            throw new ExecutorException($"{nameof(GetPropertyHandler)} : Property [{selector}] has not been registered.");
        }
        return PropertyHandlers.First(next => next.handler.data.Selectors().Contains(selector)).handler;
    }

    private bool m_hasRun;

    private readonly HashSet<string> m_validSelectors = new(); // optimize lookup
    private Dictionary<int, Action> Handlers { get; } = new();
    private FlagHandlerList FlagHandlers { get; } = new();
    private PropertyHandlerList PropertyHandlers { get; } = new();
    internal record DefaultCallback(Action<DefaultContext>? Callback, List<DefaultContext> Contexts);
    private DefaultCallback DefaultHandler { get; set; } = new(null, new List<DefaultContext>());
}