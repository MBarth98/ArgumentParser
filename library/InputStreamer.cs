namespace ArgumentParser;

public class InputStreamer
{
    private readonly string m_original;
    private string m_current;
    private int m_savedIndex = 0;
    private int m_consumed = 0;

    public InputStreamer(string input)
    {
        m_original = input;
        m_current = input;
    }

    public void Save()
    {
        m_savedIndex = m_consumed;
    }

    public void Restore()
    {
        Reset();
        Next(m_savedIndex);
    }

    private void Move(int length)
    {
        m_consumed += length;
    }

    public int ReadIndex => m_consumed;
    
    public void Reset()
    {
        m_current = m_original;
        m_consumed = 0;
    }

    public bool HasNext()
    {
        return m_current.Length > 0;
    }

    public Box<char> Peek()
    {
        if (HasNext())
        {
            return m_current[0];
        }

        return Box.None();
    }

    public Box<char> Peek(int index)
    {
        if (HasNext() && m_current.Length > index)
        {
            return m_current[index];
        }

        return Box.None();
    }

    public Box<char> Next()
    {
        if (HasNext())
        {
            var c = m_current[0];
            m_current = m_current[1..];
         
            Move(1);
            return c;
        }

        return Box.None();
    }
    
    public Box<char> NextIf(char c)
    {
        if (HasNext() && Peek() == c)
        {
            return Next();
        }

        return Box.None();
    }

    public Box<char> NextIf(Func<char, bool> predicate)
    {
        if (HasNext() && predicate(Peek()))
        {
            return Next();
        }

        return Box.None();
    }

    public Box<string> Next(int length)
    {
        if (HasNext() && m_current.Length >= length)
        {
            var s = m_current[..length];
            m_current = m_current[length..];

            Move(length);
            return s;
        }

        return Box.None();
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

    public Box<int> IndexOfNext(char c)
    {
        if (HasNext())
        {
            var index = m_current.IndexOf(c);
            if (index != -1)
            {
                return index;
            }
        }

        return Box.None();
    }


    public Box<string> Until(Func<char, bool> stop)
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

        return Box.None();
    }

    public Box<string> Until(char c)
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

            return Box.None();
        }

        return Box.None();
    }
}