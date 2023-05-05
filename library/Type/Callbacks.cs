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

    internal record ActionCallback(List<(ActionHandler Callback, List<ActionContext> Contexts)> Callbacks);
    internal record PropertyCallback(List<(PropertyHandler Callback, List<PropertyContext> Contexts)> Callbacks);


    internal class CallBinding
    {
        public ActionCallback FlagCallback { get; private set; }
        public PropertyCallback PropertyCallback { get; private set; }


        public CallBinding()
        {
            this.FlagCallback = new ActionCallback(new List<(ActionHandler, List<ActionContext>)>());
            this.PropertyCallback = new PropertyCallback(new List<(PropertyHandler, List<PropertyContext>)>());
        }

        public void AddActionCallback(ActionHandler callback, ActionContext context)
        {
            int index = this.FlagCallback.Callbacks.FindIndex(next => next.Callback == callback);
            if (index is not -1)
            {
                this.FlagCallback.Callbacks[index].Contexts.Add(context);
                return;
            }

            this.FlagCallback.Callbacks.Add((callback, new List<ActionContext> { context }));
        }

        public void AddActionCallback(ActionHandler callback, List<ActionContext> contexts)
        {
            int index = this.FlagCallback.Callbacks.FindIndex(next => next.Callback == callback);
            if (index is not -1)
            {
                this.FlagCallback.Callbacks[index].Contexts.AddRange(contexts);
                return;
            }

            this.FlagCallback.Callbacks.Add((callback, contexts));
        }
        
        public void AddPropertyCallback(PropertyHandler callback, PropertyContext context)
        {
            this.PropertyCallback.Callbacks.Add((callback, new List<PropertyContext> { context }));
        }

        public void AddPropertyCallback(PropertyHandler callback, List<PropertyContext> contexts)
        {
            this.PropertyCallback.Callbacks.Add((callback, contexts));
        }
    } 
}