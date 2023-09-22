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
            ActionTask task = ModuleUtility.Task.GetOrNew<ActionTask>();
            task.Add(() => Log.Debug("IInitHandler BeforeHandle " + ModuleUtility.Time.Time));
            return task;
        }

        public ITask AfterHandle()
        {
            ActionTask task = ModuleUtility.Task.GetOrNew<ActionTask>();
            task.Add(3.0f, () => Log.Debug("IInitHandler AfterHandle " + ModuleUtility.Time.Time));
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
            ActionTask task = ModuleUtility.Task.GetOrNew<ActionTask>();
            task.Add(() => Console.WriteLine("IStartHandler BeforeHandle "  + ModuleUtility.Time.Time));
            return task;
        }

        public ITask AfterHandle()
        {
            ActionTask task = ModuleUtility.Task.GetOrNew<ActionTask>();
            task.Add(() => Console.WriteLine("IStartHandler AfterHandle " + ModuleUtility.Time.Time));
            return task;
        }
    }
}
