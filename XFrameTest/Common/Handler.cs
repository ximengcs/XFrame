using XFrame.Core;
using XFrame.Modules.Config;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Tasks;
using XFrame.Modules.Times;

namespace XFrameTest
{
    class InitHandler : IInitHandler
    {
        public ITask BeforeHandle()
        {
            ActionTask task = XModule.Task.GetOrNew<ActionTask>();
            task.Add(() => Log.Debug("IInitHandler BeforeHandle " + XModule.Time.Time));
            return task;
        }

        public ITask AfterHandle()
        {
            ActionTask task = XModule.Task.GetOrNew<ActionTask>();
            task.Add(3.0f, () => Log.Debug("IInitHandler AfterHandle " + XModule.Time.Time));
            return task;
        }

        public void EnterHandle()
        {
            
        }
    }

    class StartHandler : IStartHandler
    {
        public ITask BeforeHandle()
        {
            ActionTask task = XModule.Task.GetOrNew<ActionTask>();
            task.Add(() => Console.WriteLine("IStartHandler BeforeHandle "  + XModule.Time.Time));
            return task;
        }

        public ITask AfterHandle()
        {
            ActionTask task = XModule.Task.GetOrNew<ActionTask>();
            task.Add(() => Console.WriteLine("IStartHandler AfterHandle " + XModule.Time.Time));
            return task;
        }
    }
}
