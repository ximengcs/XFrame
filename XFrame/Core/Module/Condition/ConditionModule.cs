using XFrame.Core;
using XFrame.Modules.Event;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;
using XFrame.Modules.XType;
using System;

namespace XFrame.Modules.Conditions
{
    [XModule]
    public class ConditionModule : SingletonModule<ConditionModule>
    {
        private IEventSystem m_Event;
        private IConditionHelper m_Helper;
        private Dictionary<int, IConditionCompare> m_Compares;
        private Dictionary<string, ConditionGroupHandle> m_Groups;

        public IEventSystem Event => m_Event;

        protected override void OnInit(object data)
        {
            base.OnInit(data);

            m_Event = EventModule.Inst.NewSys();
            m_Compares = new Dictionary<int, IConditionCompare>();
            m_Groups = new Dictionary<string, ConditionGroupHandle>();

            TypeSystem typeSys = TypeModule.Inst.GetOrNew<IConditionCompare>();
            foreach (Type type in typeSys)
            {
                IConditionCompare compare = (IConditionCompare)Activator.CreateInstance(type);
                m_Compares.Add(compare.Target, compare);
            }
        }

        public ConditionGroupHandle Register(string name, ArrayParser<PairParser<IntParser, UniversalParser>> conditions)
        {
            if (m_Groups.TryGetValue(name, out ConditionGroupHandle group))
                return group;

            group = new ConditionGroupHandle(name, conditions, InnerGroupCompleteHandler);
            m_Groups.Add(name, group);
            return group;
        }

        public void UnRegister(string name)
        {
            if (m_Groups.TryGetValue(name, out ConditionGroupHandle handle))
            {
                m_Groups.Remove(handle.Name);
                handle.Dispose();
            }
        }

        public void UnRegister(ConditionGroupHandle handle)
        {
            UnRegister(handle.Name);
        }

        private void InnerGroupCompleteHandler(ConditionGroupHandle group)
        {
            if (m_Helper != null)
            {
                if (!m_Helper.CheckFinish(group.Name))
                    m_Helper.MarkFinish(group.Name);
            }
            m_Groups.Remove(group.Name);
            group.Dispose();
        }

        public void SetHelper(IConditionHelper helper)
        {
            m_Helper = helper;
        }

        public bool CheckFinish(string groupName)
        {
            if (m_Helper != null)
                return m_Helper.CheckFinish(groupName);
            return false;
        }

        internal bool InnerCheckFinish(ConditionHandle info)
        {
            if (m_Compares.TryGetValue(info.Target, out IConditionCompare compare))
                return compare.CheckFinish(info);
            Log.Error("Condition", $"Target {info.Target} compare is null");
            return false;
        }

        internal bool InnerCheckCompare(ConditionHandle info, object param)
        {
            if (m_Compares.TryGetValue(info.Target, out IConditionCompare compare))
                return compare.Check(info, param);
            Log.Error("Condition", $"Target {info.Target} compare is null");
            return false;
        }
    }
}
