namespace ArgumentParser.Type
{
    public abstract class Parameter : Identifier
    {
        public Parameter(string name, int precedence, params string[] selectors)
        {
            m_name = name;
            foreach (var selector in selectors)
            {
                m_selectors.Add(selector);
            }

            m_precedence = precedence;
        }

        /// <summary>
        ///  set the description of the option
        /// </summary>
        /// <param name="description"></param>
        public void Description(string description) => m_description = description;

        /// <summary>
        /// make the key required (at least one instance of the key is required)
        /// </summary>
        /// <param name="isRequired">default: false</param>
        public void Required(bool isRequired = false) => m_isRequired = isRequired;

        /// <summary>
        /// allow multiple instances of the same key (default: false)
        /// </summary>
        /// <param name="allowMultiple">default: false</param>
        public void AllowMultiple(bool allowMultiple = false) => m_allowMultiple = allowMultiple;

        public void AddSelector(string selector) => m_selectors.Add(selector);

        public void RemoveSelector(string selector) => m_selectors.Remove(selector);


        internal int Precedence() => m_precedence;
        internal bool IsRequired() => m_isRequired;
        internal bool AllowMultiple() => m_allowMultiple;
        internal string Name() => m_name;
        internal string[] Selectors() => m_selectors.ToArray();
        internal string Description() => m_description;

        private bool m_allowMultiple;
        private bool m_isRequired = false;
        private string m_description = "";

        private readonly int m_precedence = -1;
        private readonly SelectorSet m_selectors = new();
        private readonly string m_name;
    }
}