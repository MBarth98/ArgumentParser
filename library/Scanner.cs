using ArgumentParser.Context;
using ArgumentParser.Type;

namespace ArgumentParser;

public class Scanner
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

    private Executor Parse()
    {
        foreach (var arg in this.m_args)
        {

        }

        return this.m_executor;
    }

    private void ParseNext(Group next, List<string> rest)
    {
        
    }

    private readonly List<string> m_args;
    private readonly Executor m_executor;
    private int m_argument_count = 0;
    private Group m_group;
}

