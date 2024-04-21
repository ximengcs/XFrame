using XFrame.Core;
using XFrame.Modules.Tasks;
using XFrame.Modules.Times;

namespace XFrame.Tasks
{
    public class XTaskHelper
    {
        private static ITaskModule m_Module;
        private static ITimeModule m_TimeModule;
        private static XDomain m_Domain;

        public static XTaskCancelToken UseToken { get; set; }
        public static XTaskAction UseAction { get; set; }

        public static ITimeModule Time => m_TimeModule;
        public static XDomain Domain => m_Domain;

        public const int MIN_PROGRESS = 0;
        public const int MAX_PROGRESS = 1;

        public static void SetDomain(XDomain domain)
        {
            m_Domain = domain;
            m_Module = domain.GetModule<ITaskModule>();
            m_TimeModule = domain.GetModule<ITimeModule>();
        }

        public static void Register(IUpdater task)
        {
            m_Module.Register(task);
        }

        public static void UnRegister(IUpdater task)
        {
            m_Module.UnRegister(task);
        }
    }
}