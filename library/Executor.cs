﻿using ArgumentParser.Context;
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

        foreach (var action in SortActions())
        {
            if (action() is not CallbackState.CONTINUE)
            {
                return;
            }
        }
    }

    public IEnumerable<Func<CallbackState>> SortActions()
    {
        var actions = m_functionBindings.Actions;
        var properties = m_functionBindings.Properties;

        List<int> precedences = new();

        foreach (var (callback, _) in actions.Callbacks)
        {
            precedences.Add(callback.Data.Precedence());
        }

        foreach (var (callback, _) in properties.Callbacks)
        {
            precedences.Add(callback.Data.Precedence());
        }

        precedences = precedences.Distinct().OrderByDescending(x => x).ToList();

        foreach (var precedence in precedences)
        {
            foreach (var (callback, contexts) in actions.Callbacks)
            {
                if (callback.Data.Precedence() == precedence)
                {
                    foreach (var context in contexts)
                    {
                        yield return () => callback.Callback(context);
                    }
                }
            }

            foreach (var (callback, contexts) in properties.Callbacks)
            {
                if (callback.Data.Precedence() == precedence)
                {
                    foreach (var context in contexts)
                    {
                        yield return () => callback.Callback(context);
                    }
                }
            }
        }
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


    internal bool MayHaveAny(string input)
    {
        try 
        {
            return MayHaveAction(input) || MayHaveProperty(input);
        } 
        catch { return false; }
    }

    internal bool HasAny(string input)
    {
        try 
        {
            return HasAction(input) || HasProperty(input);
        } 
        catch (Exception e)
        {
            Console.WriteLine(e);  
            return false; 
        }
    }

    internal bool MayHaveAction(string input)
    {
        try 
        {
            foreach (var (Callback, Contexts) in m_functionBindings.Actions.Callbacks)
            {
                foreach (var selector in Callback.Data.Selectors())
                {
                    if (selector.StartsWith(input))
                    {
                        return true;
                    }
                }
            }
            return false;
        } catch {
            return false; 
        }
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
            foreach (var (Callback, Contexts) in m_functionBindings.Properties.Callbacks)
            {
                foreach (var selector in Callback.Data.Selectors())
                {
                    if (selector.StartsWith(input))
                    {
                        return true;
                    }
                }
            }
            return false;
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

    internal void AddContext(Option token)
    {
        throw new NotImplementedException();
    }

    private bool m_hasRun;

    private readonly CallBinding m_functionBindings = new();
}