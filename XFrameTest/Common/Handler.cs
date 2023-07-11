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
            ActionTask task = TaskModule.Inst.GetOrNew<ActionTask>();
            task.Add(() => Log.Debug("IInitHandler BeforeHandle " + TimeModule.Inst.Time));
            return task;
        }

        public ITask AfterHandle()
        {
            DelayTask task = TaskModule.Inst.GetOrNew<DelayTask>();
            task.Add(3.0f, () => Log.Debug("IInitHandler AfterHandle " + TimeModule.Inst.Time));
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
            ActionTask task = TaskModule.Inst.GetOrNew<ActionTask>();
            task.Add(() => Console.WriteLine("IStartHandler BeforeHandle "  + TimeModule.Inst.Time));
            return task;
        }

        public ITask AfterHandle()
        {
            ActionTask task = TaskModule.Inst.GetOrNew<ActionTask>();
            task.Add(() => Console.WriteLine("IStartHandler AfterHandle " + TimeModule.Inst.Time));
            return task;
        }
    }
}
