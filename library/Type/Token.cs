using ArgumentParser.Context;

namespace ArgumentParser.Type
{
    public class Token
    {
        internal enum Type
        {
            PROPERTY,
            ACTION
        }

        public string name;
        public int assosiativity;
        public List<string> selectors;
        internal Type type;
        public dynamic action;

        public Token(PropertyAction action, string name, int assosiativity, params string[] selectors) 
        {
            this.name = name;
            this.assosiativity = assosiativity;
            this.selectors = new List<string>(selectors);
            this.action = action;
            this.type = Type.PROPERTY;
        }

        public Token(FlagAction action, string name, int assosiativity, params string[] selectors)
        {
            this.name = name;
            this.assosiativity = assosiativity;
            this.selectors = new List<string>(selectors);
            this.action = action;
            this.type = Type.ACTION;
        }
    }
}