
using System;
using System.Collections.Generic;

namespace XFrame.Modules.Resource
{
    public abstract class ResourceHelperBase : IResourceHelper
    {
        protected bool m_IsInit;

        public abstract List<object> DumpAll();
        public abstract object Load(string resPath, Type type);
        public abstract T Load<T>(string resPath);
        public abstract ResLoadTask LoadAsync(string resPath, Type type);
        public abstract ResLoadTask<T> LoadAsync<T>(string resPath);
        public virtual void SetResDirectHelper(IResRedirectHelper helper)
        {
            ResourceHelperBase helperBase = helper as ResourceHelperBase;
            if (helperBase != null && !helperBase.m_IsInit)
                helperBase.OnInit(null);
        }

        public abstract void Unload(object target);
        public abstract void UnloadAll();

        /// <summary>
        /// 资源加载初始化生命周期
        /// </summary>
        /// <param name="rootPath">资源根路径</param>
        protected internal abstract void OnInit(string rootPath);
    }
}
