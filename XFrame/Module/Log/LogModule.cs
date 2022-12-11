using System;
using System.Collections.Generic;
using XFrame.Core;

namespace XFrame.Modules
{
    public class LogModule : SingleModule<LogModule>
    {
        private List<ILogger> m_Loggers;

        public override void OnInit(object data)
        {
            base.OnInit(data);
            m_Loggers = new List<ILogger>();
        }

        public void Register<T>() where T : ILogger
        {
            m_Loggers.Add(Activator.CreateInstance<T>());
        }

        public void Debug(params object[] content)
        {
            foreach (ILogger logger in m_Loggers)
                logger.Debug(content);
        }
        public void Warning(params object[] content)
        {
            foreach (ILogger logger in m_Loggers)
                logger.Warning(content);
        }
        public void Error(params object[] content)
        {
            foreach (ILogger logger in m_Loggers)
                logger.Error(content);
        }
        public void Fatal(params object[] content)
        {
            foreach (ILogger logger in m_Loggers)
                logger.Fatal(content);
        }
    }
}
