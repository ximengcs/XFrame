namespace XFrame.Tasks
{
    /// <summary>
    /// ����״̬
    /// </summary>
    public enum XTaskState
    {
        /// <summary>
        /// ��Ч
        /// </summary>
        None,

        /// <summary>
        /// ����
        /// </summary>
        Normal,

        /// <summary>
        /// ȡ��
        /// </summary>
        Cancel,

        /// <summary>
        /// ����ȡ��
        /// </summary>
        BinderDispose,

        /// <summary>
        /// ������ȡ��
        /// </summary>
        ChildCancel
    }
}