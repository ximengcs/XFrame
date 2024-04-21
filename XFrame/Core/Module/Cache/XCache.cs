using System;
using XFrame.Core;
using XFrame.Modules.Event;
using XFrame.Modules.Reflection;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;
using XFrame.Collections;
using XFrame.Modules.Pools;

namespace XFrame.Modules.Caches
{
    [CoreModule]
    [XType(typeof(IPoolModule))]
    public partial class XCache : ModuleBase
    {
        private Dictionary<Type, ObjectCollection> m_Factorys;
        private IEventSystem m_EvtSys;

        public IEventSystem Event => m_EvtSys;

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

        public T GetFactory<T>() where T : class, ICacheObjectFactory
        {
            return GetFactory(typeof(T)) as T;
        }

        public ICacheObjectFactory GetFactory(Type type)
        {
            if (m_Factorys.TryGetValue(type, out ObjectCollection collection))
            {
                return collection.Factory;
            }
            return default;
        }

        public bool Check<T>() where T : ICacheObject
        {
            return Check(typeof(T));
        }

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

        public T Require<T>() where T : ICacheObject
        {
            return (T)Require(typeof(T));
        }

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
