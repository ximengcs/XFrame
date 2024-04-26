using System;
using XFrame.Core;
using XFrame.Collections;
using XFrame.Modules.Config;
using System.Collections.Generic;
using XFrame.Tasks;

namespace XFrame.Modules.Resource
{
    /// <inheritdoc/>
    [BaseModule]
    [XType(typeof(IResModule))]
    public class ResModule : ModuleBase, IResModule
    {
        #region Inner Fields
        private IResourceHelper m_ResHelper;
        private Dictionary<string, object> m_PreLoadRes;
        #endregion

        #region Life Fun
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_PreLoadRes = new Dictionary<string, object>();
            if (IsDefaultModule && !string.IsNullOrEmpty(XConfig.DefaultRes))
            {
                Type type = Domain.TypeModule.GetType(XConfig.DefaultRes);
                SetHelper(type);
            }
        }

        public IResourceHelper SetHelper(Type type)
        {
            m_ResHelper = Domain.TypeModule.CreateInstance(type) as IResourceHelper;
            m_ResHelper.OnInit(XConfig.ResPath);
            return m_ResHelper;
        }
        #endregion

        #region Interface
        /// <inheritdoc/>
        public IResourceHelper Helper => m_ResHelper;

        /// <inheritdoc/>
        public async XTask Preload(IEnumerable<string> resPaths, Type type)
        {
            foreach (string path in resPaths)
            {
                if (m_PreLoadRes.ContainsKey(path))
                    continue;
                object asset = await LoadAsync(path, type);
                if (asset != null)
                    m_PreLoadRes.Add(path, asset);
            }
        }

        /// <inheritdoc/>
        public async XTask Preload(string resPath, Type type)
        {
            if (m_PreLoadRes.ContainsKey(resPath))
                return;
            object asset = await LoadAsync(resPath, type);
            if (asset != null)
                m_PreLoadRes.Add(resPath, asset);
        }

        /// <inheritdoc/>
        public object Load(string resPath, Type type)
        {
            if (m_PreLoadRes.TryGetValue(resPath, out object asset))
                return asset;
            else
                return m_ResHelper.Load(resPath, type);
        }

        /// <inheritdoc/>
        public T Load<T>(string resPath)
        {
            if (m_PreLoadRes.TryGetValue(resPath, out object asset))
            {
                return (T)asset;
            }
            else
            {
                return m_ResHelper.Load<T>(resPath);
            }
        }

        /// <inheritdoc/>
        public ResLoadTask LoadAsync(string resPath, Type type)
        {
            return m_ResHelper.LoadAsync(resPath, type);
        }

        /// <inheritdoc/>
        public ResLoadTask<T> LoadAsync<T>(string resPath)
        {
            return m_ResHelper.LoadAsync<T>(resPath);
        }

        /// <inheritdoc/>
        public void Unload(object target)
        {
            m_ResHelper.Unload(target);
        }

        /// <inheritdoc/>
        public void UnloadPre(string resPath)
        {
            if (m_PreLoadRes.TryGetValue(resPath, out object asset))
            {
                Unload(asset);
                m_PreLoadRes.Remove(resPath);
            }
        }

        /// <inheritdoc/>
        public void UnloadAll()
        {
            m_ResHelper.UnloadAll();
        }

        /// <inheritdoc/>
        public void UnloadAllPre()
        {
            foreach (object asset in m_PreLoadRes.Values)
                m_ResHelper.Unload(asset);
            m_PreLoadRes.Clear();
        }
        #endregion
    }
}