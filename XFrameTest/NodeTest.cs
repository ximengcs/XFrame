using XFrame.Core;
using XFrame.Module.Rand;
using XFrame.Modules.Tasks;
using XFrame.Modules.XType;

namespace XFrameTest
{
    public class ActionTask : TaskBase
    {
        protected override void OnInit()
        {
            AddStrategy(new TaskStrategy());
            AddStrategy(new Strategy());
        }

        public ActionTask Add(Action handler)
        {
            return (ActionTask)Add(new Handler(handler));
        }

        private class Strategy : ITaskStrategy<Handler>
        {
            public void OnUse()
            {

            }

            public float Handle(ITask from, Handler handler)
            {
                handler.Act?.Invoke();
                return MAX_PRO;
            }
        }

        private class Handler : ITaskHandler
        {
            public Action Act;

            public Handler(Action act)
            {
                Act = act;
            }
        }
    }

    [TestClass]
    public class NodeTest
    {
        [TestMethod]
        public void Test1()
        {
            XCore core = XCore.Create(
                typeof(RandModule),
                typeof(TypeModule), 
                typeof(TaskModule));

            ActionTask task = TaskModule.Inst.GetOrNew<ActionTask>();
            task.Add(() => Console.WriteLine("Test"));

            ActionTask task2 = TaskModule.Inst.GetOrNew<ActionTask>();
            task2.Add(() => Console.WriteLine("Test2"));

            ActionTask task3 = TaskModule.Inst.GetOrNew<ActionTask>();
            task3.Add(() => Console.WriteLine("Test3"))
                 .Add(task2)
                 .Add(task)
                 .Start();

            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine(i);
                core.Update(1);
            }
            //XNode<int> node = new XNode<int>();
            //node.Add(1);
            //node.Add(2);
            //node.Add((child) =>
            //{
            //    return child.Value == 2;
            //}, 3);
            //
            //XNode<int> n = node.Get((node) => node.Value == 2);
            //foreach (var child in n)
            //{
            //    Console.WriteLine(child.Value);
            //}
        }
    }
}
