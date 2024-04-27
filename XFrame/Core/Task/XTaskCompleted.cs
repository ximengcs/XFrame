using System;
using System.Runtime.CompilerServices;

namespace XFrame.Tasks
{
    /// <summary>
    /// 处于完成状态的任务
    /// </summary>
    [AsyncMethodBuilder(typeof(XTaskCompletedAsyncMethodBuilder))]
    public struct XTaskCompleted : ICriticalNotifyCompletion, ITask
    {
        /// <summary>
        /// await
        /// </summary>
        /// <returns>返回当前任务</returns>
        public XTaskCompleted GetAwaiter()
        {
            return this;
        }

        /// <summary>
        /// 完成状态，返回true
        /// </summary>
        public bool IsCompleted => true;

        /// <summary>
        /// 任务进度，返回最大值
        /// </summary>
        public float Progress => XTaskHelper.MAX_PROGRESS;

        /// <summary>
        /// 任务行为，返回<see cref="XTaskAction.ContinueWhenSubTaskFailure"/>
        /// </summary>
        public XTaskAction TaskAction => XTaskAction.ContinueWhenSubTaskFailure;

        /// <summary>
        /// 设置任务行为，无效设置
        /// </summary>
        /// <param name="action">行为</param>
        /// <returns>返回当前任务</returns>
        public ITask SetAction(XTaskAction action)
        {
            return this;
        }

        /// <summary>
        /// 获取结果
        /// </summary>
        public void GetResult()
        {

        }

        /// <inheritdoc/>
        public void Coroutine()
        {
            InnerCoroutine();
        }

        private async void InnerCoroutine()
        {
            await this;
        }

        /// <inheritdoc/>
        public void Cancel(bool subTask)
        {

        }

        /// <inheritdoc/>
        public ITask Bind(ITaskBinder binder)
        {
            return this;
        }

        /// <inheritdoc/>
        public ITask OnCompleted(Action<XTaskState> hanlder)
        {
            if (hanlder != null)
                hanlder(XTaskState.Normal);

            return this;
        }

        ITask ITask.OnCompleted(Action handler)
        {
            if (handler != null)
                handler();
            return this;
        }

        /// <summary>
        /// 注册完成事件处理函数
        /// </summary>
        /// <param name="handler">处理函数</param>
        public void OnCompleted(Action handler)
        {
            if (handler != null)
                handler();
        }

        /// <summary>
        /// 注册完成事件处理函数
        /// </summary>
        /// <param name="handler">处理函数</param>
        public void UnsafeOnCompleted(Action handler)
        {
            if (handler != null)
                handler();
        }
    }
}