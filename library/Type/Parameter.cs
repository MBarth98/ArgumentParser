namespace ArgumentParser.Type
{
    public abstract class Parameter : Identifier
    {
        public Parameter(string name, int precedence, params string[] selectors)
        {
            this.m_name = name;
            foreach (string selector in selectors)
            {
                this.m_selectors.Add(selector);
            }

            this.m_precedence = precedence;
        }

        /// <summary>
        ///  set the description of the option
        /// </summary>
        /// <param name="description"></param>
        public void Description(string description)
        {
            this.m_description = description;
        }

        /// <summary>
        /// make the key required (at least one instance of the key is required)
        /// </summary>
        /// <param name="isRequired">default: false</param>
        public void Required(bool isRequired = false)
        {
            this.m_isRequired = isRequired;
        }

        /// <summary>
        /// allow multiple instances of the same key (default: false)
        /// </summary>
        /// <param name="allowMultiple">default: false</param>
        public void AllowMultiple(bool allowMultiple = false)
        {
            this.m_allowMultiple = allowMultiple;
        }

        public void AddSelector(string selector)
        {
            this.m_selectors.Add(selector);
        }

        public void RemoveSelector(string selector)
        {
            this.m_selectors.Remove(selector);
        }


        internal int Precedence() => this.m_precedence;
        internal bool IsRequired() => this.m_isRequired;
        internal bool AllowMultiple() => this.m_allowMultiple;
        internal string Name() => this.m_name;
        internal string[] Selectors() => this.m_selectors.ToArray();
        internal string Description() => this.m_description;


        private bool m_allowMultiple;
        private bool m_isRequired = false;
        private string m_description = "";


        private readonly int m_precedence = -1;
        private readonly SelectorSet m_selectors = new();
        private readonly string m_name;
    }
}