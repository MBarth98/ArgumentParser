namespace Argument;

using Argument.Context;
using Argument.Type;

public class Parser
{
    List<string> m_args;

    public Parser(string @string) : this(@string.Split(' ')) {}
    public Parser(string[] args)
    {
        this.m_args = new(args);
    }

    public void Process()
    {
        if (this.m_processed)
        {
            throw new Exception("Process() can only be called once");
        }

        this.Parse();

        foreach (var flag in this.m_flag_handlers)
        {
            if (flag.contexts.Count > 0)
            {
                foreach (var context in flag.contexts)
                {
                    m_handlers[context.index] = () => flag.handler.callback.Invoke(context);
                }
            }
        }

        foreach (var keyValues in this.m_property_handlers)
        {
            if (keyValues.contexts.Count > 0)
            {
                foreach (var keyValue in keyValues.contexts)
                {
                    m_handlers[keyValue.index] = () => keyValues.handler.callback.Invoke(keyValue);
                }
            }
        }

        if (this.m_default_handlers.Count > 0)
        {
            var @default = this.m_default_handlers.First();
            foreach (var defaultContext in @default.contexts)
            {
                m_handlers[defaultContext.index] = () => @default.handler.callback.Invoke(defaultContext);
            }
        }

        // call handlers in order of discovery
        foreach (var handler in this.m_handlers.OrderBy((next) => next.Key))
        {
            handler.Value.Invoke();
        }

        this.m_processed = true;
    }

    public void AddHandler(Flag flag, Action<FlagContext> handler)
    {
        bool valid = this.m_flag_handlers.All((next) =>
        {
            foreach (var selector in flag.Selectors())
            {
                if (next.handler.flag.Selectors().Contains(selector))
                {
                    throw new Exception("Another handler is already bound to the selector [" + selector + "]", new ArgumentException(nameof(flag)));
                }
            }
            return true;
        });

        if (valid)
        {
            this.m_flag_handlers.Add(new FlagHandler(flag, handler));
            this.m_candidates.UnionWith(flag.Selectors());
            return;
        }

        throw new Exception("Unknown error");
    }

    public void AddHandler(Property property, Action<PropertyContext> handler)
    {
        var this_keys = property.Selectors().Append(property.Key());
        bool valid = this.m_property_handlers.All((next) =>
        {
            var next_keys = next.handler.data.Selectors().Append(next.handler.data.Key());
            foreach (var key in this_keys)
            {
                if (next_keys.Contains(key))
                {
                    throw new Exception("Another handler is already bound to this key [" + key + "]", new ArgumentException(nameof(property)));
                }
            }
            return true;
        });

        if (valid)
        {
            this.m_property_handlers.Add(new PropertyHandler(property, handler));
            this.m_candidates.UnionWith(this_keys);
            return;
        }

        throw new Exception("Unknown error");
    }

    public void SetDefaultHandler(Action<DefaultContext> handler)
    {
        this.m_default_handlers.Add(new DefaultHandler(handler));
    }

    private void Parse()
    {
        var flags = m_flag_handlers.SelectMany((next) => next.handler.flag.Selectors());
        var properties = m_property_handlers.SelectMany((next) =>
        {
            return next.handler.data.Selectors().Append(next.handler.data.Key());
        });

        int real_start_index = 0;
        int real_end_index = 0;
        for (int i = 0; i < m_args.Count; i++)
        {
            string current = m_args[i];

            real_start_index = real_end_index;
            real_end_index += current.Length + (i < m_args.Count - 1 ? 1 : 0); // +1 for space if not last

            var defaultContext = new DefaultContext(m_argument_count, current);

            if (flags.Contains(current))
            {
                var flag = m_flag_handlers.First((next) => next.handler.flag.Selectors().Contains(current));
                m_flag_handlers.Add(flag.handler, new FlagContext(defaultContext, flag.handler.flag.Name()));
                m_argument_count++;
            }
            else if (IsProperty(i, properties, out var property))
            {
                List<string> values = current.Split(property?.data.Delimiter() ?? string.Empty).ToList();
                if (values.Count == 1 && i <= m_args.Count - 1)
                {
                    values.Add(m_args[++i]);
                }

                var pair = new KeyValuePair<string, string>(values[0], values[1]);
                property?.data.Value(pair.Value);
                m_property_handlers.Add(property, new PropertyContext(defaultContext, pair.Key, pair.Value));
                m_argument_count++;
            }
            else
            {
                m_default_handlers.Add(null, defaultContext);
                m_argument_count++;
            }
        }
    }

    private bool IsProperty(int index, IEnumerable<string> properties, out PropertyHandler? property)
    {
        property = null;
        if (properties.Any((next) => this.m_args[index].StartsWith(next)))
        {
            if (index >= this.m_args.Count )
            {
                return false;
            }

            property = m_property_handlers.First((next) =>
            {
                return this.m_args[index].StartsWith(next.handler.data.Key());
            }).handler;

            return true;
        }

        return false;
    }


    private Dictionary<int, Action> m_handlers = new();
    private FlagHandlerList m_flag_handlers = new();
    private PropertyHandlerList m_property_handlers = new();
    private DefaultHandlerList m_default_handlers = new();

    private HashSet<string> m_candidates = new();
    private int m_argument_count = 0;
    private bool m_processed = false;
}

