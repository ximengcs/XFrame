using System;
using System.Runtime.CompilerServices;

namespace XFrame.Tasks
{
    /// <summary>
    /// �������״̬������
    /// </summary>
    [AsyncMethodBuilder(typeof(XTaskCompletedAsyncMethodBuilder))]
    public struct XTaskCompleted : ICriticalNotifyCompletion, ITask
    {
        /// <summary>
        /// await
        /// </summary>
        /// <returns>���ص�ǰ����</returns>
        public XTaskCompleted GetAwaiter()
        {
            return this;
        }

        /// <summary>
        /// ���״̬������true
        /// </summary>
        public bool IsCompleted => true;

        /// <summary>
        /// ������ȣ��������ֵ
        /// </summary>
        public float Progress => XTaskHelper.MAX_PROGRESS;

        /// <summary>
        /// ������Ϊ������<see cref="XTaskAction.ContinueWhenSubTaskFailure"/>
        /// </summary>
        public XTaskAction TaskAction => XTaskAction.ContinueWhenSubTaskFailure;

        /// <summary>
        /// ����������Ϊ����Ч����
        /// </summary>
        /// <param name="action">��Ϊ</param>
        /// <returns>���ص�ǰ����</returns>
        public ITask SetAction(XTaskAction action)
        {
            return this;
        }

        /// <summary>
        /// ��ȡ���
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
        /// ע������¼�������
        /// </summary>
        /// <param name="handler">������</param>
        public void OnCompleted(Action handler)
        {
            if (handler != null)
                handler();
        }

        /// <summary>
        /// ע������¼�������
        /// </summary>
        /// <param name="handler">������</param>
        public void UnsafeOnCompleted(Action handler)
        {
            if (handler != null)
                handler();
        }
    }
}