using System.Collections.Generic;

namespace XFrame.Modules
{
    public class CDTimer
    {
        private Dictionary<int, CDInfo> m_Times;

        public CDTimer()
        {
            m_Times = new Dictionary<int, CDInfo>();
        }

        public void Record(int key, float cd)
        {
            CDInfo info = new CDInfo();
            info.CD = cd;
            info.EndTime = TimeModule.Inst.Time;
            m_Times[key] = info;
        }

        public void Reset(int key)
        {
            if (m_Times.TryGetValue(key, out CDInfo info))
                info.Mark();
        }

        public bool Check(int key, bool reset = false)
        {
            if (m_Times.TryGetValue(key, out CDInfo info))
            {
                if (info.Due)
                {
                    if (reset)
                        info.Mark();
                    return true;
                }
            }

            return false;
        }

        private class CDInfo
        {
            public float EndTime;
            public float CD;

            public bool Due
            {
                get { return TimeModule.Inst.Time >= EndTime; }
            }

            public void Mark()
            {
                EndTime = TimeModule.Inst.Time + CD;
            }

            public void Reset()
            {
                EndTime = TimeModule.Inst.Time;
            }
        }
    }
}