using XFrame.Core;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Times;
using XFrame.Tasks;

namespace XFrameTest
{
    class InitHandler : IInitHandler
    {
        public async XTask BeforeHandle()
        {
            Log.Debug("IInitHandler BeforeHandle " + Entry.GetModule<ITimeModule>().Time);
            await new XTaskCompleted();
        }

        public async XTask AfterHandle()
        {
            Log.Debug("IInitHandler AfterHandle " + Entry.GetModule<ITimeModule>().Time);
            await new XTaskCompleted();
        }

        public void EnterHandle()
        {

        }
    }

    class StartHandler : IStartHandler
    {
        public async XTask BeforeHandle()
        {
            Console.WriteLine("IStartHandler BeforeHandle "  + Entry.GetModule<ITimeModule>().Time);
            await new XTaskCompleted();
        }

        public async XTask AfterHandle()
        {
            Console.WriteLine("IStartHandler AfterHandle " + Entry.GetModule<ITimeModule>().Time);
            await new XTaskCompleted();
        }
    }
}
