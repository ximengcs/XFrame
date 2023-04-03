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
            Console.WriteLine(content[0]);
        }

        public void Fatal(params object[] content)
        {
            Console.WriteLine(content[0]);
        }

        public void Warning(params object[] content)
        {
            Console.WriteLine(content[0]);
        }
    }
}
