using System;

namespace XFrame.Modules.Tasks
{
    public partial class ActionTask : TaskBase
    {
        protected override void OnInit()
        {
            AddStrategy(new Strategy());
            AddStrategy(new BoolStrategy());
            AddStrategy(new DelayStrategy());
            AddStrategy(new NextFrameStrategy());
            AddStrategy(new ProgressStrategy());
            AddStrategy(new RepeatStrategy());
        }

        public ActionTask Add(Action handler)
        {
            return (ActionTask)Add(new Handler(handler));
        }

        public ActionTask AddNext(Action handler)
        {
            return (ActionTask)Add(new NextFrameHandler(handler));
        }

        public ActionTask Add(Func<bool> handler)
        {
            return (ActionTask)Add(new BoolHandler(handler));
        }

        public ActionTask Add(float delayTime, Action callback)
        {
            return (ActionTask)Add(new DelayHandler(delayTime, callback));
        }

        public ActionTask Add(Func<float> handler)
        {
            return (ActionTask)Add(new ProgressHandler(handler));
        }

        public ActionTask Add(float timeGap, Func<bool> handler)
        {
            return (ActionTask)Add(new RepeatHandler(timeGap, handler));
        }
    }
}
