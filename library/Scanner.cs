namespace ArgumentParser;

using ArgumentParser.Context;
using ArgumentParser.Type;

public class Scanner
{
    public Executor Executor { get; }


    public Scanner(string @string, Executor executor) : 
        this(@string.Split(' '), executor) {}

    public Scanner(IEnumerable<string> args, Executor executor)
    {
        this.m_args = new List<string>(args);
        this.Executor = executor;
    }

    public void ParseAndCommit()
    {
        this.Parse().Commit();
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

            if (Executor.HasAny(current))
            {
                if (Executor.IsExact(current))
                {
                    if (Executor.HasFlag(current))
                    {
                        // can only be a flag
                        Executor.AddContext(current, new FlagContext(defaultContext));
                        m_argument_count++;
                    }
                    else if (Executor.HasProperty(current))
                    {
                        // can only be a property (with null or space as delimiter)
                        var property = Executor.GetPropertyHandler(current);
                        var values = current.Split(property?.data.Delimiter() ?? string.Empty).ToList();
                        if (values.Count == 1 && i <= m_args.Count - 1)
                        {
                            values.Add(m_args[++i]);
                        }

                        var pair = new KeyValuePair<string, string>(values[0], values[1]);
                        property?.data.Value(pair.Value);
                        Executor.AddContext(property, new PropertyContext(defaultContext, pair.Key, pair.Value));
                        m_argument_count++;
                    }
                }
                else if (Executor.HasProperty(current))
                {
                    // can only be a property (with immediate delimiter)
                    var property = Executor.GetPropertyHandler(current);
                    var values = current.Split(property?.data.Delimiter() ?? string.Empty).ToList();
                    if (values.Count == 1 && i <= m_args.Count - 1)
                    {
                        values.Add(m_args[++i]);
                    }

                    var pair = new KeyValuePair<string, string>(values[0], values[1]);
                    property?.data.Value(pair.Value);
                    Executor.AddContext(property, new PropertyContext(defaultContext, pair.Key, pair.Value));
                    m_argument_count++;
                }
            }
            else
            {
                // does not match any flag or property
                Executor.AddContext(defaultContext);
                m_argument_count++;
            }

        }
        
        return this.Executor;
    }


    private int m_argument_count = 0;
    private readonly List<string> m_args;

}

