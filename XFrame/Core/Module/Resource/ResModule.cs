using System;
using XFrame.Core;
using XFrame.Modules.Tasks;
using XFrame.Modules.XType;
using XFrame.Modules.Config;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;

namespace XFrame.Modules.Resource
{
    /// <summary>
    /// 资源模块
    /// </summary>
    [BaseModule]
    [RequireModule(typeof(TaskModule))]
    public class ResModule : SingletonModule<ResModule>
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
            if (!string.IsNullOrEmpty(XConfig.DefaultRes))
            {
                Type type = TypeModule.Inst.GetType(XConfig.DefaultRes);
                InnerSetHelper(type);
                m_ResHelper.OnInit(XConfig.ResPath);
            }
        }

        protected IResourceHelper InnerSetHelper(Type type)
        {
            m_ResHelper = Activator.CreateInstance(type) as IResourceHelper;
            return m_ResHelper;
        }
        #endregion

        #region Interface
        /// <summary>
        /// 资源预加载(在预加载完成之后，可调用同步方法直接获取资源，在一些不支持同步方法的接口可预加载以便必要时同步加载资源)
        /// </summary>
        /// <param name="resPaths">需要加载的资源列表</param>
        /// <param name="types">资源类型列表</param>
        /// <returns>加载任务</returns>
        public ITask Preload(string[] resPaths, Type[] types)
        {
            if (m_PreLoadRes == null)
                m_PreLoadRes = new Dictionary<string, object>();
            if (resPaths.Length != types.Length)
            {
                Log.Error("XFrame", $"Preload res path is not equal types");
                return default;
            }

            XTask allTask = TaskModule.Inst.GetOrNew<XTask>();
            int count = resPaths.Length;
            for (int i = 0; i < count; i++)
            {
                string path = resPaths[i];
                Type type = types[i];
                ResLoadTask loadTask = LoadAsync(path, type);
                loadTask.OnComplete((asset) => m_PreLoadRes.Add(path, asset));
                allTask.Add(loadTask);
            }
            return allTask;
        }

        /// <summary>
        /// 加载资源(同步)
        /// </summary>
        /// <param name="resPath">资源路径</param>
        /// <param name="type">资源类型</param>
        /// <returns>加载到的资源</returns>
        public object Load(string resPath, Type type)
        {
            if (m_PreLoadRes.TryGetValue(resPath, out object asset))
                return asset;
            else
                return m_ResHelper.Load(resPath, type);
        }

        /// <summary>
        /// 加载资源(同步)
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="resPath">资源路径</param>
        /// <returns>加载到的资源</returns>
        public T Load<T>(string resPath)
        {
            if (m_PreLoadRes.TryGetValue(resPath, out object asset))
                return (T)asset;
            else
                return m_ResHelper.Load<T>(resPath);
        }

        /// <summary>
        /// 加载资源(异步)
        /// </summary>
        /// <param name="resPath">资源路径</param>
        /// <param name="type">资源类型</param>
        /// <returns>加载任务</returns>
        public ResLoadTask LoadAsync(string resPath, Type type)
        {
            return m_ResHelper.LoadAsync(resPath, type);
        }

        /// <summary>
        /// 加载资源(异步)
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="resPath">资源路径</param>
        /// <returns>加载任务</returns>
        public ResLoadTask<T> LoadAsync<T>(string resPath)
        {
            return m_ResHelper.LoadAsync<T>(resPath);
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="target">要卸载的目标</param>
        public void Unload(object target)
        {
            m_ResHelper.Unload(target);
        }

        /// <summary>
        /// 卸载预加载的资源
        /// </summary>
        /// <param name="resPath">资源路径</param>
        public void UnloadPre(string resPath)
        {
            if (m_PreLoadRes.TryGetValue(resPath, out object asset))
            {
                Unload(asset);
                m_PreLoadRes.Remove(resPath);
            }
        }

        /// <summary>
        /// 卸载所有已经加载的资源
        /// </summary>
        public void UnloadAll()
        {
            m_ResHelper.UnloadAll();
        }

        /// <summary>
        /// 卸载所有预加载的资源
        /// </summary>
        public void UnloadAllPre()
        {
            foreach (object asset in m_PreLoadRes.Values)
                m_ResHelper.Unload(asset);
            m_PreLoadRes.Clear();
        }
        #endregion
    }
}