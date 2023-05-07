using System.Globalization;
using System;
using ArgumentParser.Type;

namespace ArgumentParser;

public sealed class Scanner
{
    public Scanner(IEnumerable<string> args, Group group) 
    : this(string.Join(" ", args), new Executor(), group) { }
    public Scanner(IEnumerable<string> args, Executor executor, Group group)
    : this(string.Join(" ", args), executor, group) { }
    public Scanner(string @string, Group group) 
    : this(@string, new Executor(), group) { }
    
    public Scanner(string @string, Executor executor, Group group)
    : this(group) 
    {
        m_streamer = new InputStreamer(@string);        
        m_executor = executor;
        RegisterTokenGroup("root", m_group);
    }

    private Scanner(Group group) 
    {
        m_executor = null!;
        m_streamer = null!;

        if (string.IsNullOrWhiteSpace(group.name))
        {
            m_group = group;
            return;
        }

        m_group = new Group();
        m_group.Add(group);
    }

    public void CallHandlers()
    {
        Parse().Execute();
    }

    private Executor Parse()
    {
        ParseParameters();
        return m_executor;
    }

    private void ParseParameters()
    {
        List<Token> tokens = new();
        while (m_streamer.HasNext())
        {
            m_streamer.SkipIf(char.IsWhiteSpace);

            var c = m_streamer.Peek();
            Console.WriteLine(m_streamer.Until(char.IsWhiteSpace));

        }
    }

    private void RegisterTokenGroup(string parent, Group group)
    {
        foreach (var token in group.Tokens)
        {
            if (!parent.EndsWith(token.name))
            {
                RegisterToken(token);
            }
        }

        foreach (var subGroup in group.SubGroups)
        {
            RegisterTokenGroup($"{parent}.{subGroup.name}", subGroup);
        }
    }

    private void RegisterToken(Token token)
    {
        switch (token.type)
        {
            case Token.Type.ACTION:
                m_executor.AddHandler((ActionValue)token.value, token.action);
                break;
            case Token.Type.PROPERTY:
                m_executor.AddHandler((PropertyValue)token.value, token.action);
                break;
        }
    }
    
    private readonly InputStreamer m_streamer;
    private readonly List<Token> m_foundTokens = new();
    private readonly Executor m_executor;
    private readonly Group m_group;
}

