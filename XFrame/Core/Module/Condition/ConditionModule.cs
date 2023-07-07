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
        private Dictionary<int, IConditionHelper> m_Helpers;
        private Dictionary<int, IConditionCompare> m_Compares;
        private Dictionary<string, ConditionGroupHandle> m_Groups;

        public IEventSystem Event => m_Event;

        protected override void OnInit(object data)
        {
            base.OnInit(data);

            m_Event = EventModule.Inst.NewSys();
            m_Compares = new Dictionary<int, IConditionCompare>();
            m_Groups = new Dictionary<string, ConditionGroupHandle>();
            m_Helpers = new Dictionary<int, IConditionHelper>();

            TypeSystem typeSys = TypeModule.Inst.GetOrNew<IConditionCompare>();
            foreach (Type type in typeSys)
            {
                IConditionCompare compare = (IConditionCompare)TypeModule.Inst.CreateInstance(type);
                m_Compares.Add(compare.Target, compare);
            }
        }

        public ConditionGroupHandle Get(string name)
        {
            if (m_Groups.TryGetValue(name, out ConditionGroupHandle group))
                return group;
            return default;
        }

        public ConditionGroupHandle Register(ConditionSetting setting)
        {
            if (m_Groups.TryGetValue(setting.Name, out ConditionGroupHandle group))
                return group;
            m_Helpers.TryGetValue(setting.UseHelper, out IConditionHelper helper);
            group = new ConditionGroupHandle(setting, helper, InnerGroupCompleteHandler);
            m_Groups.Add(setting.Name, group);
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
            ConditionSetting setting = group.Setting;
            if (setting.AutoRemove)
            {
                m_Groups.Remove(group.Name);
                group.Dispose();
            }
        }

        public void AddHelper(IConditionHelper helper)
        {
            InnerAddHelper(helper);
        }

        public void AddHelper<T>() where T : IConditionHelper
        {
            InnerAddHelper(TypeModule.Inst.CreateInstance<T>());
        }

        public void AddHelper(Type type)
        {
            InnerAddHelper((IConditionHelper)TypeModule.Inst.CreateInstance(type));
        }

        private void InnerAddHelper(IConditionHelper helper)
        {
            m_Helpers.Add(helper.Type, helper);
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
