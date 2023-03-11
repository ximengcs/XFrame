using XFrame.Core;
using XFrame.Modules.Tasks;
using XFrame.Modules.Times;

namespace XFrameTest
{
    class InitHandler : IInitHandler
    {
        public ITask BeforeHandle()
        {
            ActionTask task = TaskModule.Inst.GetOrNew<ActionTask>();
            //task.Add(() => Console.WriteLine("IInitHandler BeforeHandle " + TimeModule.Inst.Time));
            return task;
        }

        public ITask AfterHandle()
        {

            DelayTask task = TaskModule.Inst.GetOrNew<DelayTask>();
            task.Add(3.0f, () => Console.WriteLine("IInitHandler AfterHandle " + TimeModule.Inst.Time));
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

    [TestClass]
    public class BaseModuleInit
    {
        [TestMethod]
        public void Test1()
        {
            Entry.Init();
            Entry.Start();

            for (int i = 0; i < 1000000; i++)
            {
                Entry.Update(0.0001f);
            }

            Entry.ShutDown();
        }
    }
}
