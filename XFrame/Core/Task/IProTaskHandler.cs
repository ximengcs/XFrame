namespace XFrame.Tasks
{
    public interface IProTaskHandler
    {
        /// <summary>
        /// 加载到的数据
        /// </summary>
        object Data { get; }

        /// <summary>
        /// 是否加载完成
        /// </summary>
        bool IsDone { get; }

        /// <summary>
        /// 加载进度
        /// </summary>
        float Pro { get; }

        /// <summary>
        /// 任务取消生命周期函数
        /// </summary>
        void OnCancel();
    }
}