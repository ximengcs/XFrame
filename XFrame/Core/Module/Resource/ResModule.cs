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
    [CoreModule]
    public class ResModule : SingletonModule<ResModule>
    {
        private IResourceHelper m_ResHelper;
        private Dictionary<string, object> m_PreLoadRes;

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

        public void Unload(object target)
        {
            m_ResHelper.Unload(target);
        }

        public void UnloadAll()
        {
            m_ResHelper.UnloadAll();
        }

        protected IResourceHelper InnerSetHelper(Type type)
        {
            m_ResHelper = Activator.CreateInstance(type) as IResourceHelper;
            return m_ResHelper;
        }
    }
}