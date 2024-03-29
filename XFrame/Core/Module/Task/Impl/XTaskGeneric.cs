﻿using System;

namespace XFrame.Modules.Tasks
{
    public class XTask<T> : TaskBase
    {
        private Action<T> m_Complete;

        public T Data { get; set; }

        protected override void OnInit()
        {

        }

        public XTask<T> OnComplete(Action<T> complete)
        {
            m_Complete += complete;
            return this;
        }

        protected override void InnerComplete()
        {
            base.InnerComplete();
            m_Complete?.Invoke(Data);
            m_Complete = null;
        }
    }
}
