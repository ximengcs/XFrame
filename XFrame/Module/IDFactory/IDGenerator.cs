using System;

namespace XFrame.Core
{
    public class IDGenerator : SingleModule<IDGenerator>
    {
        private int m_Time;
        private int m_Count;

        public override void OnInit(object data)
        {
            base.OnInit(data);
            m_Time =(int)(DateTime.Now.Ticks / 1000);
        }

        public int Next()
        {
            return m_Time + m_Count++;
        }
    }
}