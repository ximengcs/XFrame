using XFrame.Modules.Diagnotics;

namespace XFrameTest
{
    public class ConsoleLogger : ILogger
    {
        public void Debug(params object[] content)
        {
            Console.WriteLine(content[0]);
        }

        public void Error(params object[] content)
        {
            throw new NotImplementedException();
        }

        public void Fatal(params object[] content)
        {
            throw new NotImplementedException();
        }

        public void Warning(params object[] content)
        {
            throw new NotImplementedException();
        }
    }
}
