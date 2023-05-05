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
        m_functionBindings.Actions.Callbacks.Add((new ActionHandler(flag, action), new List<ActionContext>()));
    }

    public void AddHandler(PropertyValue property, PropertyFunction action)
    {
        m_functionBindings.Properties.Callbacks.Add((new PropertyHandler(property, action), new List<PropertyContext>()));
    }


    internal void AddContext(ActionHandler handler, ActionContext context)
    {
        try
        {
            m_functionBindings.AddActionCallback(handler, context);
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
            m_functionBindings.AddPropertyCallback(handler, context);
        } 
        catch (NullReferenceException) 
        {
            throw new ExecutorException($"No property handler found for selector {handler.Data.Text}");
        }
        catch { throw; } // catch all other exceptions
    }


    internal bool HasAny(string input)
    {
        try 
        {
            return HasAction(input) || HasProperty(input);
        } 
        catch { return false; }
    }

    internal bool HasAction(string input)
    {
        try 
        {
            return m_functionBindings.Actions.Callbacks.Any((x) => x.Callback.Data.Selectors().Contains(input));
        } catch { return false; }
    }

    internal bool MayHaveProperty(string input)
    {
        try 
        {
            return m_functionBindings.Properties.Callbacks.Any((x) => x.Callback.Data.Selectors().Any((y) => y.StartsWith(input)));
        } catch { return false; }
    }

    internal bool HasProperty(string input)
    {
        try 
        {
            return m_functionBindings.Properties.Callbacks.Any((x) => x.Callback.Data.Selectors().Contains(input));
        } catch { return false; }
    }

    internal ActionHandler GetActionHandler(string input)
    {
        try
        {
            return m_functionBindings.Actions.Callbacks.Find((x) => x.Callback.Data.Selectors().Contains(input)).Callback;
        } 
        catch (NullReferenceException)
        {
            throw new ExecutorException($"No flag handler found for selector {input}");
        }
        catch { throw; } // catch all other exceptions
    }

    internal PropertyHandler GetPropertyHandler(string input)
    {
        try
        {
            return m_functionBindings.Properties.Callbacks.Find((x) => x.Callback.Data.Selectors().Contains(input)).Callback;
        }
        catch (NullReferenceException)
        {
            throw new ExecutorException($"No property handler found for selector {input}");
        }
        catch { throw; } // catch all other exceptions
    }

    private bool m_hasRun;

    private readonly CallBinding m_functionBindings = new();
}