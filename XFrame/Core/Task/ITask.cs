using System;

namespace XFrame.Tasks
{
    /// <summary>
    /// ����
    /// </summary>
    public interface ITask
    {
        /// <summary>
        /// �Ƿ����
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// ����
        /// </summary>
        double Progress { get; }

        /// <summary>
        /// ������Ϊ
        /// </summary>
        XTaskAction TaskAction { get; }

        /// <summary>
        /// ������Э�̷�ʽִ��
        /// </summary>
        void Coroutine();

        /// <summary>
        /// ����������Ϊ
        /// </summary>
        /// <param name="action">��Ϊ</param>
        /// <returns>���ص�ǰ����</returns>
        ITask SetAction(XTaskAction action);
        
        /// <summary>
        /// �󶨶���
        /// </summary>
        /// <param name="binder">����</param>
        /// <returns>���ص�ǰ����</returns>
        ITask Bind(ITaskBinder binder);
        
        /// <summary>
        /// ȡ������,δ��ʼִ�е����񲻻ᱻȡ��
        /// </summary>
        /// <param name="subTask">�Ƿ�ȡ��������</param>
        void Cancel(bool subTask);

        /// <summary>
        /// ע����ɻص��¼�
        /// </summary>
        /// <param name="hanlder">�ص�������</param>
        /// <returns>���ص�ǰ����</returns>
        ITask OnCompleted(Action<XTaskState> hanlder);

        /// <summary>
        /// ע����ɻص��¼�
        /// </summary>
        /// <param name="handler">�ص�������</param>
        /// <returns>���ص�ǰ����</returns>
        ITask OnCompleted(Action handler);
    }
}