using System;
using XFrame.Modules.XType;
using XFrame.Modules.Event;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;

namespace XFrame.Core.Caches
{
    public static partial class XCache
    {
        private static Dictionary<Type, ObjectCollection> m_Factorys;
        private static IEventSystem m_EvtSys;

        public static IEventSystem Event => m_EvtSys;

        public static ICollection<ObjectCollection> Collections
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

        public static void Initialize()
        {
            Entry.OnRun += InnerInitFactory;
        }

        private static void InnerInitFactory()
        {
            Entry.OnRun -= InnerInitFactory;
            m_EvtSys = EventModule.Inst.NewSys();
            m_Factorys = new Dictionary<Type, ObjectCollection>();
            TypeSystem typeSys = TypeModule.Inst.GetOrNew<ICacheObjectFactory>();
            foreach (Type type in typeSys)
            {
                CacheObjectAttribute attr = TypeModule.Inst.GetAttribute<CacheObjectAttribute>(type);
                if (attr != null)
                {
                    if (m_Factorys.ContainsKey(type))
                    {
                        Log.Debug("XFrame", $"Cache object factory duplicate {type.FullName}, auto ignore");
                        continue;
                    }
                    ICacheObjectFactory factory = (ICacheObjectFactory)TypeModule.Inst.CreateInstance(type);
                    m_Factorys.Add(attr.Target, new ObjectCollection(attr.Target, factory, attr.CacheCount));
                }
            }
        }

        public static T GetFactory<T>() where T : class, ICacheObjectFactory
        {
            return GetFactory(typeof(T)) as T;
        }

        public static ICacheObjectFactory GetFactory(Type type)
        {
            if (m_Factorys.TryGetValue(type, out ObjectCollection collection))
            {
                return collection.Factory;
            }
            return default;
        }

        public static bool Check<T>() where T : ICacheObject
        {
            return Check(typeof(T));
        }

        public static bool Check(Type type)
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

        public static T Require<T>() where T : ICacheObject
        {
            return (T)Require(typeof(T));
        }

        public static ICacheObject Require(Type type)
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
