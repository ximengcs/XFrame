
namespace XFrame.Modules.ID
{
    internal class DefaultIDNumberHelper : IIDNumberHelper
    {
        private int m_Count;

        public int Next()
        {
            return --m_Count;
        }
    }
}
