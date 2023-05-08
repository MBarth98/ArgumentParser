using ArgumentParser.Context;

namespace ArgumentParser.Type
{
    public enum CallbackState
    {
        CONTINUE,
        STOP,
        WARNING,
        FATAL_ERROR,
    }
    
    public delegate CallbackState PropertyFunction(PropertyContext context);
    public delegate CallbackState ActionFunction(ActionContext context);

    public record PropertyHandler(PropertyValue Data, PropertyFunction Callback);
    public record ActionHandler(ActionValue Data, ActionFunction Callback);

    internal record Actions(List<(ActionHandler Callback, List<ActionContext> Contexts)> Callbacks);
    internal record Properties(List<(PropertyHandler Callback, List<PropertyContext> Contexts)> Callbacks);

    internal class CallBinding
    {
        public Actions Actions { get; private set; }
        public Properties Properties { get; private set; }


        public CallBinding()
        {
            Actions = new Actions(new List<(ActionHandler, List<ActionContext>)>());
            Properties = new Properties(new List<(PropertyHandler, List<PropertyContext>)>());
        }

        public void AddActionCallback(ActionHandler callback, ActionContext context)
        {
            int index = Actions.Callbacks.FindIndex(next => next.Callback == callback);
            if (index is not -1)
            {
                Actions.Callbacks[index].Contexts.Add(context);
                return;
            }

            Actions.Callbacks.Add((callback, new List<ActionContext> { context }));
        }

        public void AddActionCallback(ActionHandler callback, List<ActionContext> contexts)
        {
            int index = Actions.Callbacks.FindIndex(next => next.Callback == callback);
            if (index is not -1)
            {
                Actions.Callbacks[index].Contexts.AddRange(contexts);
                return;
            }

            Actions.Callbacks.Add((callback, contexts));
        }
        
        public void AddPropertyCallback(PropertyHandler callback, PropertyContext context)
        {
            Properties.Callbacks.Add((callback, new List<PropertyContext> { context }));
        }

        public void AddPropertyCallback(PropertyHandler callback, List<PropertyContext> contexts)
        {
            Properties.Callbacks.Add((callback, contexts));
        }
    } 
}