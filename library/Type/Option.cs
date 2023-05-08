using ArgumentParser.Context;

namespace ArgumentParser.Type
{
    public class Option
    {
        internal enum Type
        {
            PROPERTY,
            ACTION
        }

        public string name;
        public dynamic value;
        public dynamic action;
        internal Type type;

        public Option(PropertyFunction action, PropertyValue property) 
        {
            name = property.Name();
            value = property;
            type = Type.PROPERTY;
            this.action = action;
        }

        public Option(ActionFunction action, ActionValue flag)
        {
            name = flag.Name();
            value = flag;
            type = Type.ACTION;
            this.action = action;
        }
    }
}