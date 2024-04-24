using System;
using XFrame.Core;
using XFrame.Collections;
using XFrame.Modules.Event;
using XFrame.Modules.Reflection;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;

namespace XFrame.Modules.Caches
{
    /// <summary>
    /// 缓存模块
    /// </summary>
    [CoreModule]
    [XType(typeof(XCache))]
    public partial class XCache : ModuleBase
    {
        private Dictionary<Type, ObjectCollection> m_Factorys;
        private IEventSystem m_EvtSys;

        /// <summary>
        /// 事件系统
        /// </summary>
        public IEventSystem Event => m_EvtSys;

        /// <summary>
        /// 缓存对象集合
        /// </summary>
        public ICollection<ObjectCollection> Collections
        {
            get
            {
                if (m_Factorys == null)
                {
                    return new List<ObjectCollection>();
                }

                return m_Factorys.Values;
            }
        }

        /// <inheritdoc/>
        protected override void OnStart()
        {
            base.OnStart();
            InnerInitFactory();
        }

        private void InnerInitFactory()
        {
            m_EvtSys = Domain.GetModule<IEventModule>().NewSys();
            m_Factorys = new Dictionary<Type, ObjectCollection>();
            TypeSystem typeSys = Domain.TypeModule.GetOrNew<ICacheObjectFactory>();
            foreach (Type type in typeSys)
            {
                CacheObjectAttribute attr = Domain.TypeModule.GetAttribute<CacheObjectAttribute>(type);
                if (attr != null)
                {
                    if (m_Factorys.ContainsKey(type))
                    {
                        Log.Debug("XFrame", $"Cache object factory duplicate {type.FullName}, auto ignore");
                        continue;
                    }
                    ICacheObjectFactory factory = (ICacheObjectFactory)Domain.TypeModule.CreateInstance(type);
                    m_Factorys.Add(attr.Target, new ObjectCollection(attr.Target, factory, attr.CacheCount));
                }
            }
        }

        /// <summary>
        /// 获取对象工厂
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>工厂实例</returns>
        public T GetFactory<T>() where T : class, ICacheObjectFactory
        {
            return GetFactory(typeof(T)) as T;
        }

        /// <summary>
        /// 获取对象工厂
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <returns>工厂实例</returns>
        public ICacheObjectFactory GetFactory(Type type)
        {
            if (m_Factorys.TryGetValue(type, out ObjectCollection collection))
            {
                return collection.Factory;
            }
            return default;
        }

        /// <summary>
        /// 检查是否存在工厂
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>true为存在</returns>
        public bool Check<T>() where T : ICacheObject
        {
            return Check(typeof(T));
        }

        /// <summary>
        /// 检查是否存在工厂
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <returns>true为存在</returns>
        public bool Check(Type type)
        {
            if (m_Factorys.TryGetValue(type, out var collection))
            {
                return collection.HasItem;
            }
            else
            {
                Log.Debug("XFrame", $"Dont has {type.FullName} factory");
                return default;
            }
        }

        /// <summary>
        /// 请求一个对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>对象实例</returns>
        public T Require<T>() where T : ICacheObject
        {
            return (T)Require(typeof(T));
        }

        /// <summary>
        /// 请求一个对象
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <returns>对象实例</returns>
        public ICacheObject Require(Type type)
        {
            if (m_Factorys.TryGetValue(type, out var collection))
            {
                return collection.Get();
            }
            else
            {
                Log.Debug("XFrame", $"Dont has {type.FullName} factory");
                return default;
            }
        }
    }
}
