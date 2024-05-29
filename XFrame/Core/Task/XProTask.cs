using System;
using XFrame.Core;
using XFrame.Core.Binder;
using System.Runtime.CompilerServices;
using XFrame.Core.Threads;

namespace XFrame.Tasks
{
    /// <summary>
    /// 带进度状态的任务
    /// </summary>
    public class XProTask : ICriticalNotifyCompletion, IFiberUpdate, ICancelTask, ITask
    {
        /// <summary>
        /// 更新事件回调
        /// </summary>
        protected Action<double> m_OnUpdate;

        /// <summary>
        /// 完成回调
        /// </summary>
        protected Action<object> m_OnDataComplete;

        /// <summary>
        /// 完成状态
        /// </summary>
        protected XComplete<XTaskState> m_OnComplete;

        /// <summary>
        /// 任务绑定器
        /// </summary>
        protected ITaskBinder m_Binder;

        /// <summary>
        /// 任务行为
        /// </summary>
        protected XTaskAction m_TaskAction;

        /// <summary>
        /// 取消绑定器
        /// </summary>
        protected XTaskCancelToken m_CancelToken;

        /// <summary>
        /// 进度处理器
        /// </summary>
        protected IProTaskHandler m_ProHandler;

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

        /// <summary>
        /// 任务行为 
        /// </summary>
        public XTaskAction TaskAction => m_TaskAction;

        /// <summary>
        /// 设置任务行为 
        /// </summary>
        /// <param name="action">行为枚举</param>
        /// <returns>返回当前任务</returns>
        public ITask SetAction(XTaskAction action)
        {
            m_TaskAction = action;
            return this;
        }

        /// <summary>
        /// 获取结果
        /// </summary>
        /// <returns>返回结果</returns>
        public object GetResult()
        {
            return m_ProHandler.Data;
        }

        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsCompleted => m_OnComplete.IsComplete;

        /// <summary>
        /// 进度
        /// </summary>
        public double Progress => m_ProHandler.Pro;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="handler">进度处理器</param>
        /// <param name="cancelToken">取消绑定器</param>
        public XProTask(IProTaskHandler handler, XTaskCancelToken cancelToken = null)
        {
            m_ProHandler = handler;
            m_OnComplete = new XComplete<XTaskState>(XTaskState.Normal);
            m_CancelToken = cancelToken;
        }

        /// <summary>
        /// 开始任务
        /// </summary>
        protected virtual void InnerStart()
        {
            XTaskHelper.Register(this);
        }

        void IFiberUpdate.OnUpdate(double escapeTime)
        {
            if (m_CancelToken != null)
            {
                if (!m_CancelToken.Canceled && m_OnComplete.IsComplete)
                    return;
            }
            else
            {
                if (m_OnComplete.IsComplete)
                    return;
            }

            m_OnUpdate?.Invoke(Progress);
            if (m_Binder != null && m_Binder.IsDisposed)
            {
                m_OnComplete.Value = XTaskState.BinderDispose;
                InnerExecComplete();
            }
            else if (m_CancelToken != null && m_CancelToken.Canceled)
            {
                m_OnComplete.Value = XTaskState.Cancel;
                InnerExecComplete();
            }
            else if (m_ProHandler.IsDone)
            {
                m_OnComplete.Value = XTaskState.Normal;
                InnerExecComplete();
            }
        }

        /// <summary>
        /// 完成任务
        /// </summary>
        protected virtual void InnerExecComplete()
        {
            if (m_OnDataComplete != null)
            {
                m_OnDataComplete(GetResult());
                m_OnDataComplete = null;
            }
            m_OnComplete.IsComplete = true;
            SetResult();

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

        /// <summary>
        /// 绑定对象
        /// </summary>
        /// <param name="binder">绑定器</param>
        /// <returns>返回当前任务</returns>
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

            m_OnUpdate = null;
            XTaskHelper.UnRegister(this);
        }

        /// <summary>
        /// await 
        /// </summary>
        /// <returns>返回当前任务</returns>
        public XProTask GetAwaiter()
        {
            InnerStart();
            return this;
        }

        /// <inheritdoc/>
        public void Cancel(bool subTask)
        {
            InnerCancel();
        }

        private void InnerCancel()
        {
            if (m_OnComplete.IsComplete)
                return;
            m_OnComplete.IsComplete = true;

            ICancelTask cancelTask = this;
            cancelTask.Token.Cancel();
        }

        /// <summary>
        /// 注册更新事件
        /// </summary>
        /// <param name="handler">事件处理函数</param>
        /// <returns>返回当前任务</returns>
        public ITask OnUpdate(Action<double> handler)
        {
            m_OnUpdate += handler;
            return this;
        }

        /// <summary>
        /// 注册完成事件
        /// </summary>
        /// <param name="handler">事件处理函数</param>
        /// <returns>返回当前任务</returns>
        public ITask OnCompleted(Action<XTaskState> handler)
        {
            m_OnComplete.On(handler);
            return this;
        }

        /// <summary>
        /// 注册完成事件
        /// </summary>
        /// <param name="handler">事件处理函数</param>
        /// <returns>返回当前任务</returns>
        public ITask OnCompleted(Action handler)
        {
            m_OnComplete.On(handler);
            return this;
        }

        /// <summary>
        /// 注册完成事件
        /// </summary>
        /// <param name="handler">事件处理函数</param>
        /// <returns>返回当前任务</returns>
        public ITask OnCompleted(Action<object> handler)
        {
            if (m_OnComplete.IsComplete)
                handler(GetResult());
            else
                m_OnDataComplete += handler;
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