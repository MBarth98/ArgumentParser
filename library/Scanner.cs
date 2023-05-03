using ArgumentParser.Context;

namespace ArgumentParser;

public class Scanner
{
    public Scanner(string @string) : this(@string, new Executor()) { }
    public Scanner(string @string, Executor executor) : this(@string.Split(' '), executor) {}
    public Scanner(IEnumerable<string> args) : this(args, new Executor()) { }
    public Scanner(IEnumerable<string> args, Executor executor)
    {
        this.m_args = new List<string>(args);
        this.m_executor = executor;
    }

    public void CallHandlers()
    {
        this.Parse().Execute();
    }

    private Executor Parse()
    {
        var realStartIndex = 0;
        var realEndIndex = 0;
        
        for (var i = 0; i < m_args.Count; i++)
        {
            var current = m_args[i];

            realStartIndex = realEndIndex;
            realEndIndex += current.Length + (i < m_args.Count - 1 ? 1 : 0); // +1 for space if not last

            var defaultContext = new DefaultContext(m_argument_count, current);

            if (m_executor.HasAny(current))
            {
                if (m_executor.IsExact(current))
                {
                    if (m_executor.HasFlag(current))
                    {
                        // can only be a flag
                        m_executor.AddContext(current, new FlagContext(defaultContext));
                        m_argument_count++;
                    }
                    else if (m_executor.HasProperty(current))
                    {
                        // can only be a property (with null or space as delimiter)
                        var property = m_executor.GetPropertyHandler(current);
                        var values = current.Split(property?.Data.Delimiter() ?? string.Empty).ToList();
                        if (values.Count == 1 && i <= m_args.Count - 1)
                        {
                            values.Add(m_args[++i]);
                        }

                        var pair = new KeyValuePair<string, string>(values[0], values[1]);
                        property?.Data.Value(pair.Value);
                        m_executor.AddContext(property, new PropertyContext(defaultContext, pair.Key, pair.Value));
                        m_argument_count++;
                    }
                }
                else if (m_executor.HasProperty(current))
                {
                    // can only be a property (with immediate delimiter)
                    var property = m_executor.GetPropertyHandler(current);
                    var values = current.Split(property?.Data.Delimiter() ?? string.Empty).ToList();
                    if (values.Count == 1 && i <= m_args.Count - 1)
                    {
                        values.Add(m_args[++i]);
                    }

                    var pair = new KeyValuePair<string, string>(values[0], values[1]);
                    property?.Data.Value(pair.Value);
                    m_executor.AddContext(property, new PropertyContext(defaultContext, pair.Key, pair.Value));
                    m_argument_count++;
                }
            }
            else
            {
                // does not match any flag or property
                m_executor.AddContext(defaultContext);
                m_argument_count++;
            }

        }
        
        return this.m_executor;
    }

    private readonly List<string> m_args;
    private readonly Executor m_executor;
    private int m_argument_count = 0;
}

