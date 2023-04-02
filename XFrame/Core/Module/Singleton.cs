namespace XFrame.Core
{
    public class Singleton<T> where T : Singleton<T>, new()
    {
        private static T m_Inst;

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
