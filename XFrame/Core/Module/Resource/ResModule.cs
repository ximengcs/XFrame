﻿using System;
using XFrame.Core;
using XFrame.Modules.Tasks;
using XFrame.Modules.XType;
using XFrame.Modules.Config;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;
using System.Collections;
using XFrame.Collections;

namespace XFrame.Modules.Resource
{
    /// <summary>
    /// 资源模块
    /// </summary>
    [BaseModule]
    [RequireModule(typeof(TaskModule))]
    public class ResModule : SingletonModule<ResModule>, IResModule
    {
        #region Inner Fields
        private IResourceHelper m_ResHelper;
        private Dictionary<string, object> m_PreLoadRes;
        #endregion

        #region Life Fun
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            InnerEnsurePreload();
            if (!string.IsNullOrEmpty(XConfig.DefaultRes))
            {
                Type type = TypeModule.Inst.GetType(XConfig.DefaultRes);
                InnerSetHelper(type);
            }
        }

        protected IResourceHelper InnerSetHelper(Type type)
        {
            m_ResHelper = TypeModule.Inst.CreateInstance(type) as IResourceHelper;
            m_ResHelper.OnInit(XConfig.ResPath);
            return m_ResHelper;
        }
        #endregion

        #region Interface
        public ITask Preload(IEnumerable resPaths, Type type)
        {
            InnerEnsurePreload();

            XTask allTask = TaskModule.Inst.GetOrNew<XTask>();
            foreach (string path in resPaths)
            {
                if (m_PreLoadRes.ContainsKey(path))
                    continue;
                ResLoadTask loadTask = LoadAsync(path, type);
                loadTask.OnComplete((asset) => m_PreLoadRes.Add(path, asset));
                allTask.Add(loadTask);
            }
            return allTask;
        }

        public ITask Preload(IXEnumerable<string> resPaths, Type type)
        {
            return Preload((IEnumerable)resPaths, type);
        }

        public ITask Preload(string[] resPaths, Type type)
        {
            return Preload((IEnumerable)resPaths, type);
        }

        public ITask Preload<T>(IEnumerable resPaths)
        {
            InnerEnsurePreload();
            XTask allTask = TaskModule.Inst.GetOrNew<XTask>();
            foreach (string path in resPaths)
            {
                if (m_PreLoadRes.ContainsKey(path))
                    continue;
                ResLoadTask<T> loadTask = LoadAsync<T>(path);
                loadTask.OnComplete((asset) => m_PreLoadRes.Add(path, asset));
                allTask.Add(loadTask);
            }

            return allTask;
        }

        public ITask Preload<T>(string[] resPaths)
        {
            return Preload<T>((IEnumerable)resPaths);
        }

        public ITask Preload<T>(IXEnumerable<string> resPaths)
        {
            return Preload<T>((IEnumerable)resPaths);
        }

        public object Load(string resPath, Type type)
        {
            InnerEnsurePreload();
            if (m_PreLoadRes.TryGetValue(resPath, out object asset))
                return asset;
            else
                return m_ResHelper.Load(resPath, type);
        }

        public T Load<T>(string resPath)
        {
            InnerEnsurePreload();
            if (m_PreLoadRes.TryGetValue(resPath, out object asset))
                return (T)asset;
            else
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

        public void UnloadPre(string resPath)
        {
            InnerEnsurePreload();
            if (m_PreLoadRes.TryGetValue(resPath, out object asset))
            {
                Unload(asset);
                m_PreLoadRes.Remove(resPath);
            }
        }

        public void UnloadAll()
        {
            m_ResHelper.UnloadAll();
        }

        public void UnloadAllPre()
        {
            InnerEnsurePreload();
            foreach (object asset in m_PreLoadRes.Values)
                m_ResHelper.Unload(asset);
            m_PreLoadRes.Clear();
        }
        #endregion

        private void InnerEnsurePreload()
        {
            if (m_PreLoadRes == null)
                m_PreLoadRes = new Dictionary<string, object>();
        }
    }
}