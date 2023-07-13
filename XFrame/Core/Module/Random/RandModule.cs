using System;
using System.Collections.Generic;
using System.IO;
using XFrame.Core;

namespace XFrame.Module.Rand
{
    /// <summary>
    /// 随机模块
    /// </summary>
    [BaseModule]
    public class RandModule : SingletonModule<RandModule>
    {
        #region Inner Fields
        private HashSet<char> m_InValidChars;
        private Random m_Random;
        #endregion

        #region Life Fun
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_Random = new Random(DateTime.Now.Millisecond);

            char[] fChs = Path.GetInvalidFileNameChars();
            char[] pChs = Path.GetInvalidPathChars();
            m_InValidChars = new HashSet<char>((fChs.Length + pChs.Length) * 2);
            foreach (char ch in fChs)
                m_InValidChars.Add(ch);
            foreach (char ch in pChs)
                m_InValidChars.Add(ch);
        }
        #endregion

        #region Interface
        /// <summary>
        /// 随机产生 <paramref name="num"/> 长度的字符串
        /// </summary>
        /// <param name="num">字符串长度</param>
        /// <returns>字符串</returns>
        public string RandString(int num = 8)
        {
            char[] chars = new char[num];
            for (int i = 0; i < num; i++)
                chars[i] = (char)m_Random.Next(1, 128);
            return string.Concat(chars);
        }

        public string RandPath(int num = 8)
        {
            char[] chars = new char[num];
            for (int i = 0; i < num; i++)
            {
                char ch;
                do
                {
                    ch = (char)m_Random.Next(1, 128);
                } while (m_InValidChars.Contains(ch));
                chars[i] = ch;
            }
            return string.Concat(chars);
        }

        public T RandEnum<T>(params T[] exlusion) where T : Enum
        {
            Array values = typeof(T).GetEnumValues();
            List<T> target = new List<T>(values.Length);
            for (int i = 0; i <  values.Length; i++)
            {
                T value = (T)values.GetValue(i);
                bool find = false;
                foreach (T ex in exlusion)
                {
                    if (value.Equals(ex))
                    {
                        find = true;
                        break;
                    }
                }
                if (!find)
                    target.Add(value);
            }

            int index = m_Random.Next(0, target.Count);
            return target[index];
        }
        #endregion
    }
}
