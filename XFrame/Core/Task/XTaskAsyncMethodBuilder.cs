using System;
using System.Runtime.CompilerServices;
using XFrame.Modules.Diagnotics;

namespace XFrame.Tasks
{
    /// <summary>
    /// XTask异步方法构建器
    /// </summary>
    public struct XTaskAsyncMethodBuilder
    {
        private XTask m_Task;
        private ICancelTask m_CancelTask;

        /// <summary>
        /// 持有任务
        /// </summary>
        public XTask Task => m_Task;

        /// <summary>
        /// 创建构建器
        /// </summary>
        /// <returns>构建器</returns>
        public static XTaskAsyncMethodBuilder Create()
        {
            XTaskAsyncMethodBuilder builder = new XTaskAsyncMethodBuilder();
            builder.m_Task = new XTask();
            builder.m_CancelTask = builder.m_Task;
            builder.m_Task.SetAction(XTaskHelper.UseAction);
            return builder;
        }

        /// <summary>
        /// 设置任务结果
        /// </summary>
        public void SetResult()
        {
            m_Task.SetResult();
        }

        /// <summary>
        /// 开始执行状态机
        /// </summary>
        /// <typeparam name="TStateMachine">异步状态机类型</typeparam>
        /// <param name="stateMachine">异步状态机</param>
        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            InnerCheckCancel();
            stateMachine.MoveNext();
        }

        /// <summary>
        /// 等待一个任务
        /// </summary>
        /// <typeparam name="TAwaiter">等待器类型</typeparam>
        /// <typeparam name="TStateMachine">状态机类型</typeparam>
        /// <param name="awaiter">等待器, GetAwaiter返回的对象</param>
        /// <param name="stateMachine">状态机</param>
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            InnerCheckCancel();
            StateMachineWraper<TStateMachine> wraper =
                StateMachineWraper<TStateMachine>.Require(ref stateMachine, m_Task, SetResult);
            ITask task = awaiter as ITask;
            if (task == null)
            {
                Log.Warning("XFrame", "Please use new task.");
                awaiter.OnCompleted(wraper.RunNoState);
            }
            else
            {
                task.OnCompleted(wraper.Run);
                m_Task.AddChild(task);
            }
        }

        /// <summary>
        /// 等待一个任务
        /// </summary>
        /// <typeparam name="TAwaiter">等待器类型</typeparam>
        /// <typeparam name="TStateMachine">状态机类型</typeparam>
        /// <param name="awaiter">等待器, GetAwaiter返回的对象</param>
        /// <param name="stateMachine">状态机</param>
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            AwaitOnCompleted(ref awaiter, ref stateMachine);
        }

        /// <summary>
        /// 当发生异常时被调用
        /// </summary>
        /// <param name="e">异常类型</param>
        public void SetException(Exception e)
        {
            if (!(e is OperationCanceledException))
            {
                XTaskHelper.ExceptionHandler?.Invoke(e);
            }
        }

        /// <summary>
        /// 设置状态机
        /// </summary>
        /// <param name="stateMachine">状态机</param>
        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }

        private void InnerCheckCancel()
        {
            if (m_CancelTask.Binder != null)
            {
                if (m_CancelTask.Binder.IsDisposed)
                    m_CancelTask.Token.Cancel();
            }

            m_CancelTask.Token.Invoke();
        }
    }
}