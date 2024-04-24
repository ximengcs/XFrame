namespace XFrame.Tasks
{
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
        float Pro { get; }

        /// <summary>
        /// ����ȡ���������ں���
        /// </summary>
        void OnCancel();
    }
}