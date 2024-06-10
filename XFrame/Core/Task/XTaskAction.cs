namespace XFrame.Tasks
{
    /// <summary>
    /// 任务行为
    /// </summary>
    public enum XTaskAction
    {
        /// <summary>
        /// 子任务失败时直接完成
        /// </summary>
        CompleteWhenSubTaskFailure,

        /// <summary>
        /// 子任务失败时继续任务
        /// </summary>
        ContinueWhenSubTaskFailure
    }
}