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

    public delegate CallbackState DefaultAction(DefaultContext context);
    public delegate CallbackState PropertyAction(PropertyContext context);
    public delegate CallbackState FlagAction(FlagContext context);

    internal record PropertyHandler(Property Data, PropertyAction Callback);
    internal record FlagHandler(Flag Flag, FlagAction Callback);
    internal record DefaultHandler(DefaultAction Callback);

    internal record DefaultCallback(DefaultHandler Callback, List<DefaultContext> Contexts);
    internal record FlagCallback(List<(FlagHandler Callback, List<FlagContext> Contexts)> Callbacks);
    internal record PropertyCallback(List<(PropertyHandler Callback, List<PropertyContext> Contexts)> Callbacks);


    internal class CallBinding
    {
        public DefaultCallback DefaultCallback { get; private set; }
        public FlagCallback FlagCallback { get; private set; }
        public PropertyCallback PropertyCallback { get; private set; }

        private static readonly DefaultAction DefaultAction = (DefaultContext _) => CallbackState.CONTINUE;

        public CallBinding()
        {
            this.DefaultCallback = new DefaultCallback(new DefaultHandler(DefaultAction), new List<DefaultContext>());
            this.FlagCallback = new FlagCallback(new List<(FlagHandler, List<FlagContext>)>());
            this.PropertyCallback = new PropertyCallback(new List<(PropertyHandler, List<PropertyContext>)>());
        }

        private void SetDefaultCallbackHandler(DefaultHandler callback)
        {
            this.DefaultCallback = new DefaultCallback(callback, new List<DefaultContext>());
        }

        public void AddDefaultCallback(DefaultContext context)
        {
            if (this.DefaultCallback.Callback == null)
            {
                this.SetDefaultCallbackHandler(new DefaultHandler(DefaultAction));
            }

            this.DefaultCallback.Contexts.Add(context);
        }

        public void AddDefaultCallback(DefaultHandler callback, DefaultContext context)
        {
            this.SetDefaultCallbackHandler(callback);
            this.DefaultCallback.Contexts.Add(context);
        }

        public void AddFlagCallback(FlagHandler callback, FlagContext context)
        {
            int index = this.FlagCallback.Callbacks.FindIndex(next => next.Callback == callback);
            if (index is not -1)
            {
                this.FlagCallback.Callbacks[index].Contexts.Add(context);
                return;
            }

            this.FlagCallback.Callbacks.Add((callback, new List<FlagContext> { context }));
        }

        public void AddFlagCallback(FlagHandler callback, List<FlagContext> contexts)
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