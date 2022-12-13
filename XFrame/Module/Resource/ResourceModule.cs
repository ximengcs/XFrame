using System;
using XFrame.Modules;

namespace XFrame.Core
{
    public class ResModule : SingleModule<ResModule>
    {
        private IResourceHelper m_ResHelper;

        public T SetHelper<T>() where T : IResourceHelper
        {
            m_ResHelper = Activator.CreateInstance<T>();
            return (T)m_ResHelper;
        }

        public void SetResPath(string resPath)
        {
            m_ResHelper.Init(resPath);
        }

        public object Load(string resPath, Type type)
        {
            return m_ResHelper.Load(resPath, type);
        }

        public T Load<T>(string resPath)
        {
            return m_ResHelper.Load<T>(resPath);
        }

        public ResLoadTask LoadAsync(string resPath, Type type)
        {
            return m_ResHelper.LoadAsync(resPath, type);
        }

        public ResLoadTask<T> LoadAsync<T>(string resPath)
        {
            return m_ResHelper.LoadAsync<T>(resPath);
        }

        public void Unload(string target)
        {
            m_ResHelper.Unload(target);
        }

        public void UnloadAll()
        {
            m_ResHelper.UnloadAll();
        }
    }
}