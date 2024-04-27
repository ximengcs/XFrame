using System;
using XFrame.Core;
using XFrame.Modules.Tasks;
using XFrame.Modules.Times;

namespace XFrame.Tasks
{
    /// <summary>
    /// ����������
    /// </summary>
    public class XTaskHelper
    {
        private static ITaskModule m_Module;
        private static ITimeModule m_TimeModule;
        private static XDomain m_Domain;

        /// <summary>
        /// �쳣����
        /// </summary>
        public static Action<Exception> ExceptionHandler;

        /// <summary>
        /// ͨ��ȡ������
        /// </summary>
        public static XTaskCancelToken UseToken { get; set; }

        /// <summary>
        /// ͨ��������Ϊ
        /// </summary>
        public static XTaskAction UseAction { get; set; }

        /// <summary>
        /// ����ʹ��ʱ��ģ��
        /// </summary>
        public static ITimeModule Time => m_TimeModule;

        /// <summary>
        /// ������
        /// </summary>
        public static XDomain Domain => m_Domain;

        /// <summary>
        /// ������Сֵ
        /// </summary>
        public const int MIN_PROGRESS = 0;

        /// <summary>
        /// �������ֵ
        /// </summary>
        public const int MAX_PROGRESS = 1;

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="domain">��</param>
        public static void SetDomain(XDomain domain)
        {
            m_Domain = domain;
            m_Module = domain.GetModule<ITaskModule>();
            m_TimeModule = domain.GetModule<ITimeModule>();
        }

        /// <summary>
        /// ע��ɸ�������
        /// </summary>
        /// <param name="task">����</param>
        public static void Register(IUpdater task)
        {
            m_Module.Register(task);
        }

        /// <summary>
        /// ȡ��ע��ɸ�������
        /// </summary>
        /// <param name="task">����</param>
        public static void UnRegister(IUpdater task)
        {
            m_Module.UnRegister(task);
        }
    }
}