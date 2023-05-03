
namespace ArgumentParser.Type;

public class Flag : Identifier
{
    public Flag(string selector, string name)
    {
        this.m_selectors.Add(selector);
        this.m_flag_name = name;
    }

    public void AddSelector(string selector)
    {
        this.m_selectors.Add(selector);
    }

    public void RemoveSelector(string selector)
    {
        this.m_selectors.Remove(selector);
    }
    
    public void Description(string description)
    {
        this.m_flag_description = description;
    }

    public void Name(string name)
    {
        this.m_flag_name = name;
    }

    internal string Description()
    {
        return this.m_flag_description;
    }

    internal string Name()
    {
        return this.m_flag_name;
    }

    internal string[] Selectors()
    {
        return this.m_selectors.ToArray();
    }

    private readonly List<string> m_selectors = new();
    private string m_flag_description = "";
    private string m_flag_name = "";
}
