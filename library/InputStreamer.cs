namespace ArgumentParser;

public class InputStreamer
{
    private readonly string m_original;
    private string m_current;

    public InputStreamer(string input)
    {
        m_original = input;
        m_current = input;
    }

    public void Reset()
    {
        m_current = m_original;
    }

    public bool HasNext()
    {
        return m_current.Length > 0;
    }

    public Optional<char> Peek()
    {
        if (HasNext())
        {
            return m_current[0];
        }

        return Optional.None();
    }

    public Optional<char> Peek(int index)
    {
        if (HasNext() && m_current.Length > index)
        {
            return m_current[index];
        }

        return Optional.None();
    }

    public Optional<char> Next()
    {
        if (HasNext())
        {
            var c = m_current[0];
            m_current = m_current[1..];
            return c;
        }

        return Optional.None();
    }
    
    public Optional<char> NextIf(char c)
    {
        if (HasNext() && Peek() == c)
        {
            return Next();
        }

        return Optional.None();
    }

    public Optional<char> NextIf(Func<char, bool> predicate)
    {
        if (HasNext() && predicate(Peek()))
        {
            return Next();
        }

        return Optional.None();
    }

    public Optional<string> Next(int length)
    {
        if (HasNext() && m_current.Length >= length)
        {
            var s = m_current[..length];
            m_current = m_current[length..];
            return s;
        }

        return Optional.None();
    }

    public void Skip() 
    {
        Next();
    }

    public void Skip(int length)
    {
        Next(length);
    }

    public void SkipIf(Func<char, bool> predicate)
    {
        NextIf(predicate);
    }

    public Optional<int> IndexOfNext(char c)
    {
        if (HasNext())
        {
            var index = m_current.IndexOf(c);
            if (index != -1)
            {
                return index;
            }
        }

        return Optional.None();
    }


    public Optional<string> Until(Func<char, bool> stop)
    {
        string str = "";
        
        while (Peek().IsSome(x => !stop(x)))
        {
            Next().Some(c => str += c);
        }

        if (str.Length > 0)
        {
            return str;
        }

        return Optional.None();
    }

    public Optional<string> Until(char c)
    {
        if (HasNext())
        {
            string str = "";
            while (Peek().IsSome(x => x != c))
            {
                Next().Some(c => str += c);
            }

            if (str.Length > 0)
            {
                return str;
            }

            return Optional.None();
        }

        return Optional.None();
    }
}