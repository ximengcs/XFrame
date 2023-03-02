using System;

namespace XFrame.Modules.Tasks
{
    /// <summary>
    /// 任务
    /// </summary>
    public interface ITask : ITaskHandler
    {
        /// <summary>
        /// 任务状态
        /// </summary>
        bool IsComplete { get; }

        /// <summary>
        /// 任务是否开始执行
        /// </summary>
        bool IsStart { get; }

        /// <summary>
        /// 任务进度
        /// </summary>
        float Pro { get; }

        /// <summary>
        /// 初始化生命周期
        /// </summary>
        protected internal void OnInit();

        /// <summary>
        /// 更新生命周期
        /// </summary>
        protected internal void OnUpdate();

        /// <summary>
        /// 标记任务开始执行
        /// </summary>
        void Start();

        /// <summary>
        /// 添加任务, 需要有对应处理策略
        /// </summary>
        /// <param name="target">需要添加的处理器</param>
        /// <returns>当前任务，用于连续调用</returns>
        ITask Add(ITaskHandler target);

        /// <summary>
        /// 添加策略处理器
        /// </summary>
        /// <param name="strategy">需要添加的策略</param>
        /// <returns></returns>
        ITask AddStrategy(ITaskStrategy strategy);

        /// <summary>
        /// 设置完成回调 
        /// </summary>
        /// <param name="complete">回调</param>
        /// <returns>当前任务，用于连续调用</returns>
        ITask OnComplete(Action complete);

        /// <summary>
        /// 设置更新回调
        /// </summary>
        /// <param name="update">回调，参数为进度</param>
        /// <returns>当前任务，用于连续调用</returns>
        ITask OnUpdate(Action<float> update);
    }
}
