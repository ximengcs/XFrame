using System;

namespace XFrame.Tasks
{
    /// <summary>
    /// 任务
    /// </summary>
    public interface ITask
    {
        /// <summary>
        /// 是否完成
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// 进度
        /// </summary>
        double Progress { get; }

        /// <summary>
        /// 任务行为
        /// </summary>
        XTaskAction TaskAction { get; }

        /// <summary>
        /// 任务以协程方式执行
        /// </summary>
        void Coroutine();

        /// <summary>
        /// 设置任务行为
        /// </summary>
        /// <param name="action">行为</param>
        /// <returns>返回当前任务</returns>
        ITask SetAction(XTaskAction action);
        
        /// <summary>
        /// 绑定对象
        /// </summary>
        /// <param name="binder">绑定器</param>
        /// <returns>返回当前任务</returns>
        ITask Bind(ITaskBinder binder);
        
        /// <summary>
        /// 取消任务,未开始执行的任务不会被取消
        /// </summary>
        /// <param name="subTask">是否取消子任务</param>
        void Cancel(bool subTask);

        /// <summary>
        /// 注册完成回调事件
        /// </summary>
        /// <param name="hanlder">回调处理函数</param>
        /// <returns>返回当前任务</returns>
        ITask OnCompleted(Action<XTaskState> hanlder);

        /// <summary>
        /// 注册完成回调事件
        /// </summary>
        /// <param name="handler">回调处理函数</param>
        /// <returns>返回当前任务</returns>
        ITask OnCompleted(Action handler);
    }
}