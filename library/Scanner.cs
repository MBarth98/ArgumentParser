using ArgumentParser.Type;
using ArgumentParser.Context;

namespace ArgumentParser;

public sealed class Scanner
{
    public Scanner(IEnumerable<string> args, Executor executor, Options options)
    : this("", executor, options) 
    {
        if (args != null && args.Any())
        {
            m_streamer = new InputStreamer(args.Aggregate((x, y) => x += y.Contains(' ') ? $" \"{y}\"" : " " + y));
        }
    }

    public Scanner(IEnumerable<string> args, Options options) 
    : this(args, new Executor(), options) { }
    
    public Scanner(string @string, Options options) 
    : this(@string, new Executor(), options) { }
    
    public Scanner(string @string, Executor executor, Options options)
    : this(options) 
    {
        m_streamer = new InputStreamer(@string + '\0');        
        m_executor = executor;
        RegisterTokenGroup("root", m_options);
    }

    private Scanner(Options options) 
    {
        m_executor = null!;
        m_streamer = null!;

        if (string.IsNullOrWhiteSpace(options.name))
        {
            m_options = options;
            return;
        }

        m_options = new Options();
        m_options.Add(options);
    }

    public void CallHandlers()
    {
        Parse().Execute();
    }

    private Executor Parse()
    {
        while (m_streamer.HasNext())
        {
            m_streamer.SkipIf(char.IsWhiteSpace);

            m_streamer.Until(char.IsWhiteSpace).Require(m_executor.MayHaveAny)
                .Some(ParseString)
                .None(() => throw new NotImplementedException("TODO: Parsing error: Implement error handling."));
        }

        return m_executor;
    }

    private void ParseString(string str)
    {
        if (m_executor.HasAction(str))
        {
            ParseAction(str);
        }
        else if (m_executor.MayHaveProperty(str))
        {
            ParseProperty(str);
        }
        else
        {
            Console.WriteLine("error");
        }
    }

    private void ParseAction(string candidate)
    {
        if (m_executor.HasAction(candidate))
        {
            var token = m_registeredTokens.Find(x => {
                if (x.type != Option.Type.ACTION)
                {
                    return false;
                }

                var action = (ActionValue)x.value;
                return action.Selectors().Contains(candidate);
            }) ?? throw new Exception($"Unexpected error. (The token {candidate} was not found.)");
        
            var handler = m_executor.GetActionHandler(candidate);
            var context = new ActionContext(candidate, token);
            m_executor.AddContext(handler, context);
        }
    }

    private void ParseProperty(string candidate)
    {
        var candidate_stream = new InputStreamer(candidate + '\0');
        var bestSplit = candidate_stream.Until(c => c == '\0' || !char.IsLetterOrDigit(c)).Unwrap();

        if (bestSplit == null || !m_executor.HasProperty(bestSplit))
        {
            return;
        }

        var value = string.Empty;
        if (candidate.Length != candidate_stream.ReadIndex)
        {
            candidate_stream.Skip(1); // skip the delimiter
            value = candidate_stream.Until('\0').Unwrap() ?? string.Empty;
        }
        var next = ParseNextTokenAsValue();
        var remaining = next;
        while (next.Length > 0)
        {
            next = ParseNextTokenAsValue();
            remaining += " " + next;
        }
        value = remaining.Trim();
        
        if (value.StartsWith('"') && !value.EndsWith('"'))
        {
            value += m_streamer.Until('"').Unwrap() ?? string.Empty;
            value += m_streamer.Next().Unwrap();
        }

        var token = FindOption(Option.Type.PROPERTY, bestSplit) 
            ?? throw new Exception($"Unexpected error. (The token {candidate} was not found.)");

        var context = new PropertyContext(candidate + remaining, token, bestSplit, value);
        var handler = m_executor.GetPropertyHandler(bestSplit);

        m_executor.AddContext(handler, context);
    }



    private Option? FindOption(Option.Type type, string match)
    {
        return m_registeredTokens.Find(x => {
                if (x.type != type)
                {
                    return false;
                }

                var prop = (Parameter)x.value;
                return prop.Selectors().Contains(match);
            }) ?? null;
    }

    private string ParseNextTokenAsValue()
    {
        m_streamer.Save();

        m_streamer.SkipIf(char.IsWhiteSpace);
        var value = m_streamer.Until(char.IsWhiteSpace).Unwrap() ?? string.Empty;

        if (value.Length == 0 || m_executor.HasProperty(value) || m_executor.HasAction(value))
        {
            m_streamer.Restore();
            return string.Empty;
        }

        return value;
    }

    private void RegisterTokenGroup(string parent, Options group)
    {
        foreach (var token in group.Siblings)
        {
            RegisterToken(token);
        }

        foreach (var subGroup in group.Children)
        {
            RegisterTokenGroup($"{parent}.{subGroup.name}", subGroup);
        }
    }

    private void RegisterToken(Option token)
    {
        m_registeredTokens.Add(token);

        switch (token.type)
        {
            case Option.Type.ACTION:
                m_executor.AddHandler((ActionValue)token.value, token.action);
                break;
            case Option.Type.PROPERTY:
                m_executor.AddHandler((PropertyValue)token.value, token.action);
                break;
        }
    }
    
    private readonly InputStreamer m_streamer;
    private readonly List<Option> m_registeredTokens = new();
    private readonly Executor m_executor;
    private readonly Options m_options;
}

