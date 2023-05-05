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

        internal Type type;
        public dynamic action;
        public dynamic value;

        public Token(PropertyFunction action, PropertyValue property) 
        {
            this.name = property.Name();
            this.value = property;
            this.action = action;
            this.type = Type.PROPERTY;
        }

        public Token(ActionFunction action, ActionValue flag)
        {
            this.name = flag.Name();
            this.value = flag;
            this.action = action;
            this.type = Type.ACTION;
        }
    }
}