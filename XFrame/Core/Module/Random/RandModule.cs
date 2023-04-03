using System;
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
        private Random m_Random;
        #endregion

        #region Life Fun
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_Random = new Random(DateTime.Now.Millisecond);
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
        #endregion
    }
}
