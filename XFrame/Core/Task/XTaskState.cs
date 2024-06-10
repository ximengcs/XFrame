namespace XFrame.Tasks
{
    /// <summary>
    /// 任务状态
    /// </summary>
    public enum XTaskState
    {
        /// <summary>
        /// 无效
        /// </summary>
        None,

        /// <summary>
        /// 正常
        /// </summary>
        Normal,

        /// <summary>
        /// 取消
        /// </summary>
        Cancel,

        /// <summary>
        /// 绑定器取消
        /// </summary>
        BinderDispose,

        /// <summary>
        /// 子任务取消
        /// </summary>
        ChildCancel
    }
}