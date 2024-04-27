using System;
using System.Runtime.CompilerServices;

namespace XFrame.Tasks
{
    /// <summary>
    /// ���񹹽���
    /// </summary>
    public struct XTaskCompletedAsyncMethodBuilder
    {
        /// <summary>
        /// ��������
        /// </summary>
        public XTaskCompleted Task => default;

        /// <summary>
        /// �������񹹽���
        /// </summary>
        /// <returns>������</returns>
        public static XTaskCompletedAsyncMethodBuilder Create()
        {
            return new XTaskCompletedAsyncMethodBuilder();
        }

        /// <summary>
        /// ����������
        /// </summary>
        public void SetResult()
        {
            
        }

        /// <summary>
        /// ��ʼִ��״̬��
        /// </summary>
        /// <typeparam name="TStateMachine">�첽״̬������</typeparam>
        /// <param name="stateMachine">�첽״̬��</param>
        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            stateMachine.MoveNext();
        }

        /// <summary>
        /// �ȴ�һ������
        /// </summary>
        /// <typeparam name="TAwaiter">�ȴ�������</typeparam>
        /// <typeparam name="TStateMachine">״̬������</typeparam>
        /// <param name="awaiter">�ȴ���, GetAwaiter���صĶ���</param>
        /// <param name="stateMachine">״̬��</param>
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            stateMachine.MoveNext();
        }

        /// <summary>
        /// �ȴ�һ������
        /// </summary>
        /// <typeparam name="TAwaiter">�ȴ�������</typeparam>
        /// <typeparam name="TStateMachine">״̬������</typeparam>
        /// <param name="awaiter">�ȴ���, GetAwaiter���صĶ���</param>
        /// <param name="stateMachine">״̬��</param>
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            stateMachine.MoveNext();
        }

        /// <summary>
        /// �������쳣ʱ������
        /// </summary>
        /// <param name="e">�쳣����</param>
        public void SetException(Exception e)
        {
            if (!(e is OperationCanceledException))
            {
                XTaskHelper.ExceptionHandler?.Invoke(e);
            }
        }

        /// <summary>
        /// ����״̬��
        /// </summary>
        /// <param name="stateMachine">״̬��</param>
        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }
    }
}