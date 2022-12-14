﻿using System;
using System.Collections.Generic;
using XFrame.Core;

namespace XFrame.Modules
{
    /// <summary>
    /// Log模块
    /// </summary>
    public class LogModule : SingletonModule<LogModule>
    {
        private List<ILogger> m_Loggers;

        public override void OnInit(object data)
        {
            base.OnInit(data);
            m_Loggers = new List<ILogger>();
            if (XConfig.DefaultLogger != null && XConfig.DefaultLogger is ILogger)
                InnerAddLogger(XConfig.DefaultLogger);
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
