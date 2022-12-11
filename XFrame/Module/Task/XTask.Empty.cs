using XFrame.Modules;

namespace XFrame.Modules
{
    public partial class XTask
    {
        private class EmptyTask : XTask
        {
            public EmptyTask()
            {
                IsComplete = true;
                IsStart = true;
            }
        }

        private static XTask m_Empty = new EmptyTask();
        public static XTask Empty => m_Empty;
    }
}