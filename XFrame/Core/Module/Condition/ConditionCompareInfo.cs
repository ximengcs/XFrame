using System;
using System.Collections.Generic;
using System.Reflection;

namespace XFrame.Modules.Conditions
{
    internal struct CompareDelegateInfo
    {
        public Delegate Check;
        public Delegate OnEventTrigger;

        public CompareDelegateInfo(Delegate check, Delegate onEventTrigger)
        {
            Check = check;
            OnEventTrigger = onEventTrigger;
        }
    }

    internal struct CompareInfo
    {
        private static Dictionary<Type, CompareDelegateInfo> m_DeleCache;
        public IConditionCompare m_Inst;
        public CompareDelegateInfo m_DeleInfo;

        public int Target => m_Inst.Target;
        public bool Valid => m_Inst != null;

        public bool CheckFinish(IConditionHandle info)
        {
            return m_Inst.CheckFinish(info);
        }

        public bool Check(IConditionHandle info, object param)
        {
            return (bool)m_DeleInfo.Check.DynamicInvoke(info, param);
        }

        public void OnEventTrigger(object param)
        {
            m_DeleInfo.OnEventTrigger.DynamicInvoke(param);
        }

        public CompareInfo(IConditionCompare compare)
        {
            m_Inst = compare;

            Type type = compare.GetType();
            if (m_DeleCache == null)
                m_DeleCache = new Dictionary<Type, CompareDelegateInfo>();
            if (!m_DeleCache.TryGetValue(type, out m_DeleInfo))
            {
                m_DeleInfo = new CompareDelegateInfo();
                Type interfaceType = type.GetInterface(typeof(IConditionCompare<>).FullName);
                Console.WriteLine(type.Name);
                Type geneType = interfaceType.GetGenericArguments()[0];
                Console.WriteLine(geneType.FullName);
                MethodInfo methodInfo = interfaceType.GetMethod(nameof(Check));
                Type deleType = typeof(Func<,,>);
                deleType = deleType.MakeGenericType(typeof(IConditionHandle), geneType, typeof(bool));
                m_DeleInfo.Check = methodInfo.CreateDelegate(deleType, m_Inst);

                methodInfo = interfaceType.GetMethod(nameof(OnEventTrigger));
                deleType = typeof(Action<>);
                deleType = deleType.MakeGenericType(geneType);
                m_DeleInfo.OnEventTrigger = methodInfo.CreateDelegate(deleType, m_Inst);

                m_DeleCache.Add(type, m_DeleInfo);
            }
        }
    }
}
