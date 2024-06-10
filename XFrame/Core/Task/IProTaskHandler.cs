namespace XFrame.Tasks
{
    /// <summary>
    /// ��������������
    /// </summary>
    public interface IProTaskHandler
    {
        /// <summary>
        /// ���ص�������
        /// </summary>
        object Data { get; }

        /// <summary>
        /// �Ƿ�������
        /// </summary>
        bool IsDone { get; }

        /// <summary>
        /// ���ؽ���
        /// </summary>
        double Pro { get; }

        /// <summary>
        /// ����ȡ���������ں���
        /// </summary>
        void OnCancel();
    }
}