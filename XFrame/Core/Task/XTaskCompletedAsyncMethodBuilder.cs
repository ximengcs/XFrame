using System;
using System.Runtime.CompilerServices;

namespace XFrame.Tasks
{
    /// <summary>
    /// 任务构建器
    /// </summary>
    public struct XTaskCompletedAsyncMethodBuilder
    {
        /// <summary>
        /// 持有任务
        /// </summary>
        public XTaskCompleted Task => default;

        /// <summary>
        /// 创建任务构建器
        /// </summary>
        /// <returns>构建器</returns>
        public static XTaskCompletedAsyncMethodBuilder Create()
        {
            return new XTaskCompletedAsyncMethodBuilder();
        }

        /// <summary>
        /// 设置任务结果
        /// </summary>
        public void SetResult()
        {
            
        }

        /// <summary>
        /// 开始执行状态机
        /// </summary>
        /// <typeparam name="TStateMachine">异步状态机类型</typeparam>
        /// <param name="stateMachine">异步状态机</param>
        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
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
            stateMachine.MoveNext();
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
            stateMachine.MoveNext();
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
    }
}