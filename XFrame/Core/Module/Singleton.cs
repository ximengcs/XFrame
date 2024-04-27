namespace XFrame.Core
{
    /// <summary>
    /// 单例
    /// </summary>
    /// <typeparam name="T">持有类型</typeparam>
    public class Singleton<T> where T : Singleton<T>, new()
    {
        private static T m_Inst;

        /// <summary>
        /// 单例实例
        /// </summary>
        public static T Inst
        {
            get
            {
                if (m_Inst == null)
                    m_Inst = new T();
                return m_Inst; 
            }
        }
    }
}
