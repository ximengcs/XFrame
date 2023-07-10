
using System.Collections.Generic;

namespace XFrame.Modules.Diagnotics
{
    /// <summary>
    /// Log
    /// </summary>
    public static class Log
    {
        private static Dictionary<int, List<object[]>> s_WaitQueue = new Dictionary<int, List<object[]>>
        {
            { 0, new List<object[]>() },
            { 1, new List<object[]>() },
            { 2, new List<object[]>() },
            { 3, new List<object[]>() }
        };

        public static void EnqueueWaitQueue(params object[] content)
        {
            if (s_WaitQueue != null)
                s_WaitQueue[0].Add(content);
            else
                Debug(content);
        }

        public static void ConsumeWaitQueue()
        {
            if (s_WaitQueue == null)
                return;
            foreach (object[] message in s_WaitQueue[0]) Debug(message);
            foreach (object[] message in s_WaitQueue[1]) Warning(message);
            foreach (object[] message in s_WaitQueue[2]) Error(message);
            foreach (object[] message in s_WaitQueue[3]) Fatal(message);
            s_WaitQueue = null;
        }

        public static void Print(LogLevel level, params object[] content)
        {
            switch (level)
            {
                case LogLevel.Ignore: break;
                case LogLevel.Debug: Debug(content); break; 
                case LogLevel.Warning: Warning(content); break; 
                case LogLevel.Error: Error(content); break; 
                case LogLevel.Fatal: Fatal(content); break;
            }
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="content">信息</param>
        public static void Debug(params object[] content)
        {
            if (LogModule.Inst == null)
                s_WaitQueue[0].Add(content);
            else
                LogModule.Inst.Debug(content);
        }

        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="content">信息</param>
        public static void Warning(params object[] content)
        {
            if (LogModule.Inst == null)
                s_WaitQueue[1].Add(content);
            else
                LogModule.Inst.Warning(content);
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="content">信息</param>
        public static void Error(params object[] content)
        {
            if (LogModule.Inst == null)
                s_WaitQueue[2].Add(content);
            else
                LogModule.Inst.Error(content);
        }

        /// <summary>
        /// 致命错误信息
        /// </summary>
        /// <param name="content">信息</param>
        public static void Fatal(params object[] content)
        {
            if (LogModule.Inst == null)
                s_WaitQueue[3].Add(content);
            else
                LogModule.Inst.Fatal(content);
        }
    }
}
