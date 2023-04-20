using System;
using XFrame.Core;
using XFrame.Modules.XType;
using XFrame.Modules.Config;
using System.Collections.Generic;

namespace XFrame.Modules.Diagnotics
{
    /// <summary>
    /// Log模块
    /// </summary>
    [BaseModule]
    public class LogModule : SingletonModule<LogModule>
    {
        private List<ILogger> m_Loggers;

        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_Loggers = new List<ILogger>();
            if (!string.IsNullOrEmpty(XConfig.DefaultLogger))
            {
                Type type = TypeModule.Inst.GetType(XConfig.DefaultLogger);
                InnerAddLogger(type);
            }
        }

        #region Interface
        /// <summary>
        /// 添加Log辅助器
        /// </summary>
        /// <typeparam name="T">Log辅助器类型</typeparam>
        public void AddLogger<T>() where T : ILogger
        {
            m_Loggers.Add(InnerAddLogger(typeof(T)));
        }

        /// <summary>
        /// 获取Log辅助器
        /// </summary>
        /// <typeparam name="T">辅助器类型</typeparam>
        /// <returns>获取到的实例</returns>
        public T GetLogger<T>() where T : ILogger
        {
            foreach (ILogger logger in m_Loggers)
            {
                if (logger.GetType() == typeof(T))
                    return (T)logger;
            }
            return default;
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="content">信息</param>
        public void Debug(params object[] content)
        {
            foreach (ILogger logger in m_Loggers)
                logger.Debug(content);
        }

        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="content">信息</param>
        public void Warning(params object[] content)
        {
            foreach (ILogger logger in m_Loggers)
                logger.Warning(content);
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="content">信息</param>
        public void Error(params object[] content)
        {
            foreach (ILogger logger in m_Loggers)
                logger.Error(content);
        }

        /// <summary>
        /// 致命错误信息
        /// </summary>
        /// <param name="content">信息</param>
        public void Fatal(params object[] content)
        {
            foreach (ILogger logger in m_Loggers)
                logger.Fatal(content);
        }
        #endregion

        private ILogger InnerAddLogger(Type type)
        {
            ILogger logger = Activator.CreateInstance(type) as ILogger;
            m_Loggers.Add(logger);
            return logger;
        }
    }
}
