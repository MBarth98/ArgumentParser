using System.Diagnostics;
using ArgumentParser.Context;
using ArgumentParser.Type;

namespace ArgumentParser;

public sealed class Scanner
{
    public Scanner(string @string, Group group) : this(@string, new Executor(), group) { }
    public Scanner(string @string, Executor executor, Group group) : this(@string.Split(' '), executor, group) { }
    public Scanner(IEnumerable<string> args, Group group) : this(args, new Executor(), group) { }
    public Scanner(IEnumerable<string> args, Executor executor, Group group)
    : this(group)
    {
        m_args = new List<string>(args);
        m_executor = executor;
    }

    private Scanner(Group group) 
    {
        m_args = new List<string>();
        m_executor = new Executor();
        
        if (string.IsNullOrWhiteSpace(group.name))
        {
            m_group = group;
            RegisterTokenGroup("root", m_group);
            return;
        }

        m_group = new Group();
        m_group.Add(group);

        RegisterTokenGroup("root", m_group);
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
        for (int i = 0; i < m_args.Count; i++)
        {
            
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

    private readonly List<Token> m_foundTokens = new();
    private readonly List<string> m_args;
    private readonly Executor m_executor;
    private readonly Group m_group;
}

