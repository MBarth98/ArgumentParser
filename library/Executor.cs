using Argument.Type;

namespace Argument;

using Context;
using System;


public class Executor
{
    public void Commit()
    {
        if (this.m_hasRun)
        {
            throw new Exception("Process() can only be called once");
        }

        MergeHandlers();

        InvokeHandlers();

        m_hasRun = true;
    }
    private void InvokeHandlers()
    {
        // call handlers in order of discovery
        foreach (var handler in this.Handlers.OrderBy((next) => next.Key))
        {
            handler.Value.Invoke();
        }

    }

    private void MergeHandlers()
    {
        foreach (var (handler, contexts) in this.FlagHandlers)
        {
            if (contexts.Count <= 0) continue;

            foreach (var context in contexts)
            {
                Handlers[context.index] = () => handler.callback.Invoke(context);
            }
        }

        foreach (var (handler, contexts) in this.PropertyHandlers)
        {
            if (contexts.Count <= 0) continue;
            foreach (var keyValue in contexts)
            {
                Handlers[keyValue.index] = () => handler.callback.Invoke(keyValue);
            }
        }

        if (this.DefaultHandler.Callback == null) return;

        foreach (var defaultContext in DefaultHandler.Contexts)
        {
            Handlers[defaultContext.index] = () => this.DefaultHandler.Callback.Invoke(defaultContext);
        }
    }

    public void AddHandler(Flag flag, Action<FlagContext> handler)
    {
        var valid = this.FlagHandlers.All((next) =>
        {
            foreach (var selector in flag.Selectors())
            {
                if (next.handler.flag.Selectors().Contains(selector))
                {
                    throw new Exception("Another handler is already bound to the selector [" + selector + "]",
                        innerException: new ArgumentException(nameof(flag)));
                }

                this.m_validSelectors.Add(selector);
            }
            return true;
        });

        if (!valid) throw new Exception("Unknown error");

        this.FlagHandlers.Add(new FlagHandler(flag, handler));
    }

    public void AddHandler(Property property, Action<PropertyContext> handler)
    {
        var thisKeys = property.Selectors().Append(property.Key());
        var valid = this.PropertyHandlers.All((next) =>
        {
            var nextKeys = next.handler.data.Selectors().Append(next.handler.data.Key());

            foreach (var thisKey in thisKeys)
            {
                foreach (var nextKey in nextKeys)
                {
                    if (Equals(nextKey, thisKey))
                    {
                        throw new Exception("Another handler is already bound to this key [" + thisKey + "]",
                            innerException: new ArgumentException(nameof(property)));
                    }
                }

                this.m_validSelectors.Add(thisKey);
            }
            return true;
        });

        if (!valid) throw new System.Exception("Unknown error");

        this.PropertyHandlers.Add(new Context.PropertyHandler(property, handler));
    }

    public void AddHandler(object _, Action<DefaultContext> handler)
    {
        this.DefaultHandler = new DefaultCallback(handler, new List<DefaultContext>());
    }


    public List<string> GetFlagSelectors => 
        this.FlagHandlers.SelectMany((next) => 
            next.handler.flag.Selectors()).ToList();

    public List<string> GetPropertySelectors => 
        this.PropertyHandlers.SelectMany((next) => 
            next.handler.data.Selectors().Append(next.handler.data.Key())).ToList();


    internal void AddContext(FlagHandler handler, FlagContext context)
    {   
        this.FlagHandlers.Add(handler, context);
    }

    internal void AddContext(PropertyHandler handler, PropertyContext context)
    {
        this.PropertyHandlers.Add(handler, context);
    }

    internal void AddContext(string selector, FlagContext context)
    {
        if (!this.HasFlag(selector))
        {
            throw new Exception("No handler is bound to the selector [" + selector + "]");
        }
        
        this.FlagHandlers.Add(this.GetFlagHandler(selector), context);
    }

    internal void AddContext(string selector, PropertyContext context)
    {
        if (!this.HasProperty(selector))
        {
            throw new Exception("No handler is bound to the selector [" + selector + "]");
        }
        
        this.PropertyHandlers.Add(this.GetPropertyHandler(selector), context);
    }

    internal void AddContext(DefaultContext context)
    {
        this.DefaultHandler.Contexts.Add(context);
    }

    internal bool HasDefaultHandler => this.DefaultHandler.Callback != null;
    internal bool HasFlagHandler => this.FlagHandlers.Count > 0;
    internal bool HasPropertyHandler => this.PropertyHandlers.Count > 0;


    internal bool HasAny(string selector)
    {
        try
        {
            return
                this.m_validSelectors.Contains(selector) ||
                this.m_validSelectors.Any(selector.StartsWith);
        }
        catch (Exception e)
        {
            return false;
        }
    }

    internal bool IsExact(string selector)
    {
        return this.m_validSelectors.Contains(selector);
    }

    internal bool HasFlag(string selector)
    {
        return this.m_validSelectors.Contains(selector) &&
               this.FlagHandlers.Any(next => 
                   next.handler.flag.Selectors().Contains(selector));
    }

    internal bool HasProperty(string selector)
    {
        return this.m_validSelectors.Contains(selector) && 
               this.PropertyHandlers.Any(next => 
                   next.handler.data.Selectors().Contains(selector));
    }

    internal FlagHandler GetFlagHandler(string selector)
    {
        if (!this.HasFlag(selector))
        {
            throw new Exception("No handler is bound to the selector [" + selector + "]");
        }
        return this.FlagHandlers.First(next => 
                       next.handler.flag.Selectors().Contains(selector)).handler;
    }

    internal PropertyHandler GetPropertyHandler(string selector)
    {
        if (!this.HasProperty(selector))
        {
            throw new Exception("No handler is bound to the selector [" + selector + "]");
        }
        return this.PropertyHandlers.First(next => 
                                  next.handler.data.Selectors().Contains(selector)).handler;
    }

    private bool m_hasRun = false;

    private readonly HashSet<string> m_validSelectors = new(); // optimize lookup
    private Dictionary<int, Action> Handlers { get; } = new();
    private FlagHandlerList FlagHandlers { get; } = new();
    private PropertyHandlerList PropertyHandlers { get; } = new();
    internal record DefaultCallback(Action<DefaultContext>? Callback, List<DefaultContext> Contexts);
    private DefaultCallback DefaultHandler { get; set; } = new(null, new List<DefaultContext>());
}