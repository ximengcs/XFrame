using System;

namespace XFrame.Modules.Tasks
{
    public partial class ActionTask : TaskBase
    {
        protected override void OnCreateFromPool()
        {
            base.OnCreateFromPool();
            AddStrategy(new Strategy());
            AddStrategy(new BoolStrategy());
            AddStrategy(new DelayStrategy());
            AddStrategy(new ProgressStrategy());
            AddStrategy(new RepeatStrategy());
        }

        public ActionTask Add(Action handler, bool nextFrame = false)
        {
            return (ActionTask)Add(new Handler(handler, nextFrame));
        }

        public ActionTask Add(Func<bool> handler, bool nextFrame = false)
        {
            return (ActionTask)Add(new BoolHandler(handler, nextFrame));
        }

        public ActionTask Add(float delayTime, Action callback, bool nextFrame = false)
        {
            return (ActionTask)Add(new DelayHandler(delayTime, callback, nextFrame));
        }

        public ActionTask Add(Func<float> handler, bool nextFrame = false)
        {
            return (ActionTask)Add(new ProgressHandler(handler, nextFrame));
        }

        public ActionTask Add(float timeGap, Func<bool> handler, bool nextFrame = false)
        {
            return (ActionTask)Add(new RepeatHandler(timeGap, handler, nextFrame));
        }
    }
}
