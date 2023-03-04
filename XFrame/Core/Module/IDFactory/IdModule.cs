using System;
using XFrame.Core;

namespace XFrame.Modules.ID
{
    /// <summary>
    /// Id模块
    /// </summary>
    [CoreModule]
    public class IdModule : SingletonModule<IdModule>
    {
        private int m_Time;
        private int m_Count;

        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_Time =(int)(DateTime.Now.Ticks / 1000);
        }

        /// <summary>
        /// 生成一个Id
        /// </summary>
        /// <returns>生成的Id</returns>
        public int Next()
        {
            return m_Time + m_Count++;
        }
    }
}