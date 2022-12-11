using System;
using XFrame.Modules;

namespace XFrame.Core
{
    public class ResModule : SingleModule<ResModule>
    {
        private IResourceHelper m_ResHelper;

        public void Register<T>() where T : IResourceHelper
        {
            m_ResHelper = Activator.CreateInstance<T>();
        }

        public XTask LoadAllAsync()
        {
            if (m_ResHelper != null)
            {
                bool loadDone = false;
                return m_ResHelper.Init().Add(() =>
                {
                    m_ResHelper.LoadAllAsync(() => loadDone = true);
                    return loadDone;
                });
            }
            else
            {
                Log.Error($"ResHelper Init Error!");
                return default;
            }
        }

        public T Load<T>(string dir, string name) where T : class
        {
            return m_ResHelper.Load<T>(dir, name);
        }

        public T Load<T>(params string[] filePart) where T : class
        {
            return m_ResHelper.Load<T>(filePart);
        }

        public void Unload()
        {
            m_ResHelper.UnloadAll();
        }
    }
}