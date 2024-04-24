using System;

namespace XFrame.Modules.Pools
{
    internal class DefaultPoolHelper : IPoolHelper
    {
        private PoolModule m_Module;

        public int CacheCount => 64;

        public DefaultPoolHelper(PoolModule module)
        {
            m_Module = module;
        }

        IPoolObject IPoolHelper.Factory(Type type, int poolKey, object userData)
        {
            return (IPoolObject)m_Module.Domain.TypeModule.CreateInstance(type);
        }

        void IPoolHelper.OnObjectCreate(IPoolObject obj)
        {

        }

        void IPoolHelper.OnObjectDestroy(IPoolObject obj)
        {

        }

        void IPoolHelper.OnObjectRelease(IPoolObject obj)
        {

        }

        void IPoolHelper.OnObjectRequest(IPoolObject obj)
        {

        }
    }
}
