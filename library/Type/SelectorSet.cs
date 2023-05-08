using System.Collections;

namespace ArgumentParser.Type
{
    public sealed class SelectorSet : IEnumerable<string>
    {
        public SelectorSet()
        {
            m_selectors = new HashSet<string>();
        }

        public void Add(string selector) => m_selectors.Add(selector);

        public void Remove(string selector) => m_selectors.Remove(selector);

        public HashSet<string> Selectors() => m_selectors;

        private readonly HashSet<string> m_selectors;
        
        public IEnumerator<string> GetEnumerator() => m_selectors.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}