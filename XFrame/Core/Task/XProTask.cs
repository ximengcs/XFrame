using System;
using XFrame.Core;
using XFrame.Core.Binder;
using System.Runtime.CompilerServices;
using XFrame.Core.Threads;

namespace XFrame.Tasks
{
    /// <summary>
    /// ������״̬������
    /// </summary>
    public class XProTask : ICriticalNotifyCompletion, IFiberUpdate, ICancelTask, ITask
    {
        /// <summary>
        /// �����¼��ص�
        /// </summary>
        protected Action<double> m_OnUpdate;

        /// <summary>
        /// ��ɻص�
        /// </summary>
        protected Action<object> m_OnDataComplete;

        /// <summary>
        /// ���״̬
        /// </summary>
        protected XComplete<XTaskState> m_OnComplete;

        /// <summary>
        /// �������
        /// </summary>
        protected ITaskBinder m_Binder;

        /// <summary>
        /// ������Ϊ
        /// </summary>
        protected XTaskAction m_TaskAction;

        /// <summary>
        /// ȡ������
        /// </summary>
        protected XTaskCancelToken m_CancelToken;

        /// <summary>
        /// ���ȴ�����
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
        /// ������Ϊ 
        /// </summary>
        public XTaskAction TaskAction => m_TaskAction;

        /// <summary>
        /// ����������Ϊ 
        /// </summary>
        /// <param name="action">��Ϊö��</param>
        /// <returns>���ص�ǰ����</returns>
        public ITask SetAction(XTaskAction action)
        {
            m_TaskAction = action;
            return this;
        }

        /// <summary>
        /// ��ȡ���
        /// </summary>
        /// <returns>���ؽ��</returns>
        public object GetResult()
        {
            return m_ProHandler.Data;
        }

        /// <summary>
        /// �Ƿ����
        /// </summary>
        public bool IsCompleted => m_OnComplete.IsComplete;

        /// <summary>
        /// ����
        /// </summary>
        public double Progress => m_ProHandler.Pro;

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="handler">���ȴ�����</param>
        /// <param name="cancelToken">ȡ������</param>
        public XProTask(IProTaskHandler handler, XTaskCancelToken cancelToken = null)
        {
            m_ProHandler = handler;
            m_OnComplete = new XComplete<XTaskState>(XTaskState.Normal);
            m_CancelToken = cancelToken;
        }

        /// <summary>
        /// ��ʼ����
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
        /// �������
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
        /// �󶨶���
        /// </summary>
        /// <param name="binder">����</param>
        /// <returns>���ص�ǰ����</returns>
        public ITask Bind(ITaskBinder binder)
        {
            m_Binder = binder;
            return this;
        }

        /// <summary>
        /// ���ý��
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
        /// <returns>���ص�ǰ����</returns>
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
        /// ע������¼�
        /// </summary>
        /// <param name="handler">�¼�������</param>
        /// <returns>���ص�ǰ����</returns>
        public ITask OnUpdate(Action<double> handler)
        {
            m_OnUpdate += handler;
            return this;
        }

        /// <summary>
        /// ע������¼�
        /// </summary>
        /// <param name="handler">�¼�������</param>
        /// <returns>���ص�ǰ����</returns>
        public ITask OnCompleted(Action<XTaskState> handler)
        {
            m_OnComplete.On(handler);
            return this;
        }

        /// <summary>
        /// ע������¼�
        /// </summary>
        /// <param name="handler">�¼�������</param>
        /// <returns>���ص�ǰ����</returns>
        public ITask OnCompleted(Action handler)
        {
            m_OnComplete.On(handler);
            return this;
        }

        /// <summary>
        /// ע������¼�
        /// </summary>
        /// <param name="handler">�¼�������</param>
        /// <returns>���ص�ǰ����</returns>
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