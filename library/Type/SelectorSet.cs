using System.Collections;

namespace ArgumentParser.Type
{
    public sealed class SelectorSet : IEnumerable<string>
    {
        public SelectorSet()
        {
            this.m_selectors = new HashSet<string>();
        }

        public void Add(string selector)
        {
            this.m_selectors.Add(selector);
        }

        public void Remove(string selector)
        {
            this.m_selectors.Remove(selector);
        }

        public HashSet<string> Selectors()
        {
            return this.m_selectors;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return this.m_selectors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        
        private readonly HashSet<string> m_selectors;
    }
}