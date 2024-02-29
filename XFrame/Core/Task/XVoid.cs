using System;
using System.Runtime.CompilerServices;

namespace XFrame.Tasks
{
    [AsyncMethodBuilder(typeof(XVoidAsyncMethodBuilder))]
    public struct XVoid : ICriticalNotifyCompletion
    {
        public bool IsCompleted => true;

        public void Coroutine()
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