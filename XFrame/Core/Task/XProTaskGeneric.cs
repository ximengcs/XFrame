﻿using System;
using System.Runtime.CompilerServices;
using XFrame.Core;

namespace XFrame.Tasks
{
    public abstract class XProTask<T> : ICriticalNotifyCompletion, IUpdater, ICancelTask, ITask
    {
        protected Action<float> m_OnUpdate;
        protected XComplete<XTaskState> m_OnComplete;
        protected ITaskBinder m_Binder;
        protected XTaskAction m_TaskAction;
        protected XTaskCancelToken m_CancelToken;
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

        public XTaskAction TaskAction => m_TaskAction;

        public ITask SetAction(XTaskAction action)
        {
            m_TaskAction = action;
            return this;
        }

        public abstract T GetResult();

        public bool IsCompleted => m_OnComplete.IsComplete;

        public float Progress => m_ProHandler.Pro;

        public XProTask(IProTaskHandler handler, XTaskCancelToken cancelToken = null)
        {
            m_ProHandler = handler;
            m_OnComplete = new XComplete<XTaskState>(XTaskState.Normal);
            m_CancelToken = cancelToken;
            XModule.Task.Register(this);
        }

        void IUpdater.OnUpdate(float escapeTime)
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

        protected virtual void InnerExecComplete()
        {
            m_OnComplete.IsComplete = true;
            m_OnComplete.Invoke();
        }

        void ICancelTask.SetState(XTaskState state)
        {
            m_OnComplete.Value = state;
        }

        public void Coroutine()
        {
            InnerCoroutine();
        }

        private async void InnerCoroutine()
        {
            await this;
        }

        public ITask Bind(ITaskBinder binder)
        {
            m_Binder = binder;
            return this;
        }

        public void SetResult()
        {
            if (m_CancelToken != null && !m_CancelToken.Disposed)
                XTaskCancelToken.Release(m_CancelToken);

            m_OnUpdate = null;
            XModule.Task.UnRegister(this);
        }

        public XProTask<T> GetAwaiter()
        {
            return this;
        }

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


        public ITask OnUpdate(Action<float> handler)
        {
            m_OnUpdate += handler;
            return this;
        }


        public ITask OnCompleted(Action<XTaskState> handler)
        {
            m_OnComplete.On(handler);
            return this;
        }

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
