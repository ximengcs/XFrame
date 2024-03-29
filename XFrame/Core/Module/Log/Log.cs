﻿using System.Collections;
using System.Collections.Generic;

namespace XFrame.Modules.Diagnotics
{
    /// <summary>
    /// Log
    /// </summary>
    public static class Log
    {
        private struct LogInfo
        {
            public LogLevel Level;
            public object[] Content;

            public LogInfo(LogLevel level, object[] content)
            {
                Level = level;
                Content = content;
            }
        }

        public static bool ToQueue { get; set; }
        public static bool Power { get; set; }

        private static Queue<LogInfo> s_WaitQueue;

        static Log()
        {
            ToQueue = true;
            Power = true;
            s_WaitQueue = new Queue<LogInfo>(128);
        }

        public static void ConsumeWaitQueue()
        {
            if (s_WaitQueue == null)
                return;
            bool toQueue = ToQueue;
            ToQueue = false;
            while (s_WaitQueue.Count > 0)
            {
                LogInfo info = s_WaitQueue.Dequeue();
                InnerPrint(info.Level, info.Content);
            }
            ToQueue = toQueue;
            s_WaitQueue.Clear();
        }

        public static void Print(LogLevel level, params object[] content)
        {
            if (!Power)
                return;

            if (ToQueue)
            {
                s_WaitQueue.Enqueue(new LogInfo(level, content));
            }
            else
            {
                InnerPrint(level, content);
            }
        }

        private static void InnerPrint(LogLevel level, params object[] content)
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
            if (!Power)
                return;
            if (ToQueue)
                s_WaitQueue.Enqueue(new LogInfo(LogLevel.Debug, content));
            else
                LogModule.Inst.Debug(content);
        }

        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="content">信息</param>
        public static void Warning(params object[] content)
        {
            if (!Power)
                return;
            if (ToQueue)
                s_WaitQueue.Enqueue(new LogInfo(LogLevel.Warning, content));
            else
                LogModule.Inst.Warning(content);
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="content">信息</param>
        public static void Error(params object[] content)
        {
            if (!Power)
                return;
            if (ToQueue)
                s_WaitQueue.Enqueue(new LogInfo(LogLevel.Error, content));
            else
                LogModule.Inst.Error(content);
        }

        /// <summary>
        /// 致命错误信息
        /// </summary>
        /// <param name="content">信息</param>
        public static void Fatal(params object[] content)
        {
            if (!Power)
                return;
            if (ToQueue)
                s_WaitQueue.Enqueue(new LogInfo(LogLevel.Fatal, content));
            else
                LogModule.Inst.Fatal(content);
        }
    }
}
