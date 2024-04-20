using XFrame.Core;
using XFrame.Modules.Diagnotics;
using XFrame.Tasks;

namespace XFrameTest
{
    class InitHandler : IInitHandler
    {
        public async XTask BeforeHandle()
        {
            Log.Debug("IInitHandler BeforeHandle " + XModule.Time.Time);
            await new XTaskCompleted();
        }

        public async XTask AfterHandle()
        {
            Log.Debug("IInitHandler AfterHandle " + XModule.Time.Time);
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
            Console.WriteLine("IStartHandler BeforeHandle "  + XModule.Time.Time);
            await new XTaskCompleted();
        }

        public async XTask AfterHandle()
        {
            Console.WriteLine("IStartHandler AfterHandle " + XModule.Time.Time);
            await new XTaskCompleted();
        }
    }
}
