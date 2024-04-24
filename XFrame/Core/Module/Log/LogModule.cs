using System;
using XFrame.Core;
using XFrame.Collections;
using XFrame.Modules.Config;
using System.Collections.Generic;

namespace XFrame.Modules.Diagnotics
{
    /// <inheritdoc/>
    [BaseModule]
    [XType(typeof(ILogModule))]
    public class LogModule : ModuleBase, ILogModule
    {
        private List<ILogger> m_Loggers;

        /// <inheritdoc/>
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_Loggers = new List<ILogger>();
            if (!string.IsNullOrEmpty(XConfig.DefaultLogger))
            {
                Type type = Domain.TypeModule.GetType(XConfig.DefaultLogger);
                InnerAddLogger(type);
            }
        }

        #region Interface
        /// <inheritdoc/>
        public void AddLogger<T>() where T : ILogger
        {
            m_Loggers.Add(InnerAddLogger(typeof(T)));
        }

        /// <inheritdoc/>
        public T GetLogger<T>() where T : ILogger
        {
            foreach (ILogger logger in m_Loggers)
            {
                if (logger.GetType() == typeof(T))
                    return (T)logger;
            }
            return default;
        }

        /// <inheritdoc/>
        public void Debug(params object[] content)
        {
            foreach (ILogger logger in m_Loggers)
                logger.Debug(content);
        }

        /// <inheritdoc/>
        public void Warning(params object[] content)
        {
            foreach (ILogger logger in m_Loggers)
                logger.Warning(content);
        }

        /// <inheritdoc/>
        public void Error(params object[] content)
        {
            foreach (ILogger logger in m_Loggers)
                logger.Error(content);
        }

        /// <inheritdoc/>
        public void Fatal(params object[] content)
        {
            foreach (ILogger logger in m_Loggers)
                logger.Fatal(content);
        }

        /// <inheritdoc/>
        public void Exception(Exception e)
        {
            foreach (ILogger logger in m_Loggers)
                logger.Exception(e);
        }
        #endregion

        private ILogger InnerAddLogger(Type type)
        {
            ILogger logger = Domain.TypeModule.CreateInstance(type) as ILogger;
            m_Loggers.Add(logger);
            return logger;
        }
    }
}
