using System.Linq;
using ArgumentParser.Context;
using ArgumentParser.Type;
using ArgumentParser.Error.Exceptional;

namespace ArgumentParser;


public sealed class Executor
{
    public void Execute()
    {
        if (m_hasRun)
        {
            throw new ExecutorException($"{nameof(Execute)} can only be called once.");
        }


        m_hasRun = true;
    }

    public void AddHandler(ActionValue flag, ActionFunction action)
    {
        this.m_functionBindings.FlagCallback.Callbacks.Add((new ActionHandler(flag, action), new List<ActionContext>()));
    }

    public void AddHandler(PropertyValue property, PropertyFunction action)
    {
        this.m_functionBindings.PropertyCallback.Callbacks.Add((new PropertyHandler(property, action), new List<PropertyContext>()));
    }


    internal void AddContext(ActionHandler handler, ActionContext context)
    {
        try
        {
            this.m_functionBindings.AddActionCallback(handler, context);
        }
        catch (NullReferenceException)
        {
            throw new ExecutorException($"No flag handler found for selector {handler.Data.Text}");
        }
        catch { throw; } // catch all other exceptions
    }

    internal void AddContext(PropertyHandler handler, PropertyContext context)
    {
        try 
        {
            this.m_functionBindings.AddPropertyCallback(handler, context);
        } 
        catch (NullReferenceException) 
        {
            throw new ExecutorException($"No property handler found for selector {handler.Data.Text}");
        }
        catch { throw; } // catch all other exceptions
    }


    internal bool HasAny(string selector)
    {
        try 
        {
            return this.HasFlag(selector) || this.HasProperty(selector);
        } 
        catch { return false; }
    }

    internal bool HasFlag(string selector)
    {
        try 
        {
            return this.m_functionBindings.FlagCallback.Callbacks.Any((x) => x.Callback.Data.Selectors().Contains(selector));
        } catch { return false; }
    }

    internal bool HasProperty(string selector)
    {
        try 
        {
            return this.m_functionBindings.PropertyCallback.Callbacks.Any((x) => x.Callback.Data.Selectors().Contains(selector));
        } catch { return false; }
    }

    internal ActionHandler GetFlagHandler(string selector)
    {
        try
        {
            return this.m_functionBindings.FlagCallback.Callbacks.Find((x) => x.Callback.Data.Selectors().Contains(selector)).Callback;
        } 
        catch (NullReferenceException)
        {
            throw new ExecutorException($"No flag handler found for selector {selector}");
        }
        catch { throw; } // catch all other exceptions
    }

    internal PropertyHandler GetPropertyHandler(string selector)
    {
        try
        {
            return this.m_functionBindings.PropertyCallback.Callbacks.Find((x) => x.Callback.Data.Selectors().Contains(selector)).Callback;
        }
        catch (NullReferenceException)
        {
            throw new ExecutorException($"No property handler found for selector {selector}");
        }
        catch { throw; } // catch all other exceptions
    }

    private bool m_hasRun;

    private readonly CallBinding m_functionBindings = new();
}