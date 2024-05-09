using System;
using XFrame.Core;
using System.Collections.Generic;

namespace XFrame.Modules.Diagnotics
{
    /// <summary>
    /// Log
    /// </summary>
    public static class Log
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public const string XFrame = nameof(XFrame);
        public const string Container = nameof(Container);
        public const string CSV = nameof(CSV);
        public const string Condition = nameof(Condition);
        public const string Procedure = nameof(Procedure);
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释

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

        /// <summary>
        /// 是否将Log输出到队列中
        /// </summary>
        public static bool ToQueue { get; set; }

        /// <summary>
        /// 开关
        /// </summary>
        public static bool Power { get; set; }

        private static ILogModule m_Log;
        private static Queue<LogInfo> s_WaitQueue;

        static Log()
        {
            ToQueue = true;
            Power = true;
            s_WaitQueue = new Queue<LogInfo>(128);
        }

        /// <summary>
        /// 设置域
        /// </summary>
        /// <param name="domain">域</param>
        public static void SetDomain(XDomain domain)
        {
            m_Log = domain.GetModule<ILogModule>();
        }

        /// <summary>
        /// 消耗队列中的Log输出到Logger中
        /// </summary>
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

        /// <summary>
        /// 输出Log
        /// </summary>
        /// <param name="level">Log等级</param>
        /// <param name="content">信息</param>
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
                m_Log.Debug(content);
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
                m_Log.Warning(content);
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
                m_Log.Error(content);
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
                m_Log.Fatal(content);
        }

        /// <summary>
        /// 异常信息
        /// </summary>
        /// <param name="e">异常</param>
        public static void Exception(Exception e)
        {
            if (!Power)
                return;
            if (ToQueue)
                s_WaitQueue.Enqueue(new LogInfo(LogLevel.Fatal, new object[] { e }));
            else
                m_Log.Exception(e);
        }
    }
}
