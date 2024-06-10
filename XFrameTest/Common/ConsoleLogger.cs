using System;
using XFrame.Modules.Diagnotics;

namespace XFrameTest
{
    public class ConsoleLogger : ILogger
    {
        public void Debug(params object[] content)
        {
            Console.WriteLine(string.Concat(content));
        }

        public void Error(params object[] content)
        {
            Console.WriteLine(string.Concat(content));
        }

        public void Exception(Exception e)
        {
            throw new NotImplementedException();
        }

        public void Fatal(params object[] content)
        {
            Console.WriteLine(string.Concat(content));
        }

        public void Warning(params object[] content)
        {
            Console.WriteLine(string.Concat(content));
        }
    }
}
