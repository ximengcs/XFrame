using System;
using XFrame.Core;

namespace XFrame.Module.Rand
{
    [CoreModule]
    public class RandModule : SingletonModule<RandModule>
    {
        private Random m_Random;

        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_Random = new Random(DateTime.Now.Millisecond);
        }

        public string RandString(int num = 8)
        {
            char[] chars = new char[num];
            for (int i = 0; i < num; i++)
                chars[i] = (char)m_Random.Next(1, 128);
            return string.Concat(chars);
        }
    }
}
