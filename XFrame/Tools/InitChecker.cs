using System;
using XFrame.Modules.Pools;

namespace XFrame.Tools
{
    public class InitChecker : PoolObjectBase, IPoolObject
    {
        private bool m_Inited;
        private Action m_Callback;

        private InitChecker() { }

        public void Trigger()
        {
            if (m_Inited) return;
            m_Inited = true;
            m_Callback?.Invoke();
            m_Callback = null;
        }

        public void Listen(Action callback)
        {
            if (m_Inited)
            {
                callback?.Invoke();
            }
            else
            {
                m_Callback = callback;
            }
        }

        protected override void OnReleaseFromPool()
        {
            base.OnReleaseFromPool();
            m_Inited = false;
            m_Callback = null;
        }

        protected override void OnRequestFromPool()
        {
            base.OnRequestFromPool();
            m_Inited = false;
            m_Callback = null;
        }

        public static InitChecker Create()
        {

            InitChecker inst = References.Require<InitChecker>();
            return inst;
        }

        public static implicit operator bool(InitChecker checker)
        {
            return checker.m_Inited;
        }
    }
}
