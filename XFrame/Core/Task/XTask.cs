using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using XFrame.Core.Binder;

namespace XFrame.Tasks
{
    /// <inheritdoc/>
    [AsyncMethodBuilder(typeof(XTaskAsyncMethodBuilder))]
    public partial class XTask : ICriticalNotifyCompletion, ICancelTask, ITask
    {
        private XComplete<XTaskState> m_OnComplete;
        private ITaskBinder m_Binder;
        private XTaskCancelToken m_CancelToken;
        private List<ITask> m_Children;

        XTaskCancelToken ICancelTask.Token
        {
            get
            {
                if (XTaskHelper.UseToken != null)
                    m_CancelToken = XTaskHelper.UseToken;
                else if (m_CancelToken == null)
                    m_CancelToken = XTaskCancelToken.Require();
                return m_CancelToken;
            }
        }

        ITaskBinder ICancelTask.Binder => m_Binder;

        private XTaskAction m_TaskAction;
        /// <inheritdoc/>
        public XTaskAction TaskAction => m_TaskAction;

        /// <inheritdoc/>
        public ITask SetAction(XTaskAction action)
        {
            m_TaskAction = action;
            return this;
        }

        /// <inheritdoc/>
        public bool IsCompleted => m_OnComplete.IsComplete;

        /// <inheritdoc/>
        public float Progress => m_OnComplete.IsComplete ? XTaskHelper.MAX_PROGRESS : XTaskHelper.MIN_PROGRESS;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="cancelToken">取消绑定器</param>
        public XTask(XTaskCancelToken cancelToken = null)
        {
            m_OnComplete = new XComplete<XTaskState>(XTaskState.Normal);
            m_CancelToken = cancelToken;
            m_Children = new List<ITask>();
        }

        internal void AddChild(ITask task)
        {
            m_Children.Add(task);
        }

        void ICancelTask.SetState(XTaskState state)
        {
            m_OnComplete.Value = state;
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
        public ITask Bind(ITaskBinder binder)
        {
            m_Binder = binder;
            return this;
        }
        
        /// <summary>
        /// 设置结果
        /// </summary>
        public void SetResult()
        {
            if (m_CancelToken != null && !m_CancelToken.Disposed)
                XTaskCancelToken.Release(m_CancelToken);

            m_OnComplete.IsComplete = true;
        }

        /// <summary>
        /// 获取结果
        /// </summary>
        public void GetResult()
        {
        }

        /// <summary>
        /// await
        /// </summary>
        /// <returns>返回当前任务</returns>
        public XTask GetAwaiter()
        {
            return this;
        }

        /// <summary>
        /// 取消任务
        /// </summary>
        /// <param name="subTask">是否取消子任务</param>
        public void Cancel(bool subTask)
        {
            InnerCancel(subTask);
        }

        private void InnerCancel(bool subTask)
        {
            if (m_OnComplete.IsComplete)
                return;
            m_OnComplete.IsComplete = true;

            if (subTask)
            {
                foreach (ITask task in m_Children)
                {
                    task.Cancel(subTask);
                }
            }

            ICancelTask cancelTask = this;
            cancelTask.Token.Cancel();
        }

        /// <summary>
        /// 注册完成事件
        /// </summary>
        /// <param name="handler">处理函数</param>
        /// <returns>返回当前任务</returns>
        public ITask OnCompleted(Action<XTaskState> handler)
        {
            m_OnComplete.On(handler);
            return this;
        }

        /// <summary>
        /// 注册完成事件
        /// </summary>
        /// <param name="handler">处理函数</param>
        /// <returns>返回当前任务</returns>
        public ITask OnCompleted(Action handler)
        {
            m_OnComplete.On(handler);
            return this;
        }

        void INotifyCompletion.OnCompleted(Action handler)
        {
            m_OnComplete.On(handler);
        }

        void ICriticalNotifyCompletion.UnsafeOnCompleted(Action handler)
        {
            m_OnComplete.On(handler);
        }
    }
}