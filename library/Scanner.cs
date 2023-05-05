using System.Diagnostics;
using ArgumentParser.Context;
using ArgumentParser.Type;

namespace ArgumentParser;

public sealed class Scanner
{
    public Scanner(string @string, Group group) : this(@string, new Executor(), group) { }
    public Scanner(string @string, Executor executor, Group group) : this(@string.Split(' '), executor, group) {}
    public Scanner(IEnumerable<string> args, Group group) : this(args, new Executor(), group) { }
    public Scanner(IEnumerable<string> args, Executor executor, Group group)
    {
        this.m_args = new List<string>(args);
        this.m_executor = executor;
        this.m_group = group;
    }

    public void CallHandlers()
    {
        this.Parse().Execute();
    }

    private void RegisterHandlers()
    {
        this.m_tokens = this.ParseGroups("root", this.m_group);
        
        foreach (Token token in this.m_tokens.Values)
        {
            switch (token.type)
            {
                case Token.Type.ACTION:
                    this.m_executor.AddHandler((Type.ActionValue)token.value, token.action);
                    break;
                case Token.Type.PROPERTY:
                    this.m_executor.AddHandler((PropertyValue)token.value, token.action);
                    break;
            }
        }
        Console.WriteLine(m_tokens.Aggregate("", (acc, token) => $"{acc}\n{token.Key} => {token.Value.name}"));
    }

    private Executor Parse()
    {
        this.RegisterHandlers();
        return this.m_executor;
    }

    private Dictionary<string, Token> ParseGroups(string parent, Group group)
    {
        var dict = new Dictionary<string, Token>();

        foreach (var token in group.Tokens)
        {
            if (!parent.EndsWith(token.name))
            {
                dict.Add($"{parent}.{token.name}", token);
            }
        }

        foreach (var subGroup in group.SubGroups)
        {
            dict.Add($"{parent}.{subGroup.name}", subGroup.Tokens[0]);
            dict = dict.Concat(this.ParseGroups($"{parent}.{subGroup.name}", subGroup)).ToDictionary(k => k.Key, v => v.Value);
        }

        return dict;
    }

    private readonly List<Token> m_foundTokens = new();
    private readonly List<string> m_args;
    private readonly Executor m_executor;
    private readonly Group m_group;
    private Dictionary<string, Token> m_tokens = new();
}

