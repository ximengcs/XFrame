using System;
using XFrame.Core;
using XFrame.Modules.Config;
using XFrame.Modules.XType;

namespace XFrame.Modules.Resource
{
    /// <summary>
    /// 资源模块
    /// </summary>
    public class ResModule : SingletonModule<ResModule>
    {
        private IResourceHelper m_ResHelper;

        protected override void OnInit(object data)
        {
            base.OnInit(data);
            if (!string.IsNullOrEmpty(XConfig.DefaultRes))
            {
                Type type = TypeModule.Inst.GetType(XConfig.DefaultRes);
                InnerSetHelper(type);
                m_ResHelper.OnInit(XConfig.ResPath);
            }
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

        private IResourceHelper InnerSetHelper(Type type)
        {
            m_ResHelper = Activator.CreateInstance(type) as IResourceHelper;
            return m_ResHelper;
        }
    }
}