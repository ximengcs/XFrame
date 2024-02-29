using System;
using System.Runtime.CompilerServices;

namespace XFrame.Tasks
{
    [AsyncMethodBuilder(typeof(XTaskCompletedAsyncMethodBuilder))]
    public struct XTaskCompleted : ICriticalNotifyCompletion
    {
        public XTaskCompleted GetAwaiter()
        {
            return this;
        }

        public bool IsCompleted => true;

        public void GetResult()
        {
        }

        public void OnCompleted(Action continuation)
        {
        }

        public void UnsafeOnCompleted(Action continuation)
        {
        }
    }
}