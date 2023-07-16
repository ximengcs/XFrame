using System;
using XFrame.Core;
using XFrame.Modules.Event;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;
using XFrame.Modules.XType;
using XFrame.Modules.Pools;
using XFrame.Collections;
using System.Diagnostics;

namespace XFrame.Modules.Conditions
{
    /// <summary>
    /// 条件监听模块
    /// </summary>
    [XModule]
    public class ConditionModule : SingletonModule<ConditionModule>
    {
        private IEventSystem m_Event;
        private Dictionary<int, Dictionary<int, IConditionHelper>> m_Helpers;
        private Dictionary<int, Dictionary<int, CompareInfo>> m_Compares;
        private Dictionary<int, Type> m_HelpersType;
        private Dictionary<int, Type> m_ComparesType;

        private List<ConditionGroupHandle> m_GroupList;
        private Dictionary<string, ConditionGroupHandle> m_Groups;

        /// <summary>
        /// 当需要触发某个条件时，触发 <see cref="ConditionEvent"/> 事件到此事件系统
        /// </summary>
        public IEventSystem Event => m_Event;

        protected override void OnInit(object data)
        {
            base.OnInit(data);

            m_Event = EventModule.Inst.NewSys();
            m_Event.Listen(ConditionEvent.EventId, InnerConditionTouchHandler);
            m_Event.Listen(ConditionGroupEvent.EventId, InnerConditionGroupTouchHandler);
            m_Event.Listen(SpecificConditionEvent.EventId, InnerSpecificCondition);
            m_Helpers = new Dictionary<int, Dictionary<int, IConditionHelper>>();
            m_Compares = new Dictionary<int, Dictionary<int, CompareInfo>>();
            m_Groups = new Dictionary<string, ConditionGroupHandle>();
            m_HelpersType = new Dictionary<int, Type>();
            m_ComparesType = new Dictionary<int, Type>();
            m_GroupList = new List<ConditionGroupHandle>();

            TypeSystem typeSys = TypeModule.Inst.GetOrNew<IConditionCompare>();
            foreach (Type type in typeSys)
            {
                if (type.IsInterface || type.IsAbstract)
                    continue;
                IConditionCompare compare = (IConditionCompare)TypeModule.Inst.CreateInstance(type);
                m_Compares.Add(compare.Target, new Dictionary<int, CompareInfo>() { { ConditionHelperSetting.DEFAULT_INSTANCE, new CompareInfo(compare) } });
                m_ComparesType.Add(compare.Target, compare.GetType());
            }

            typeSys = TypeModule.Inst.GetOrNew<IConditionHelper>();
            foreach (Type type in typeSys)
            {
                if (type.IsInterface || type.IsAbstract)
                    continue;
                IConditionHelper helper = (IConditionHelper)TypeModule.Inst.CreateInstance(type);
                m_Helpers.Add(helper.Type, new Dictionary<int, IConditionHelper>() { { ConditionHelperSetting.DEFAULT_INSTANCE, helper } });
                m_HelpersType.Add(helper.Type, helper.GetType());
            }
        }

        internal CompareInfo GetOrNewCompare(int target, int instance = ConditionHelperSetting.DEFAULT_INSTANCE)
        {
            if (m_Compares.TryGetValue(target, out Dictionary<int, CompareInfo> compares))
            {
                if (compares.TryGetValue(instance, out CompareInfo compare))
                    return compare;

                if (m_ComparesType.TryGetValue(target, out Type type))
                {
                    IConditionCompare compareInst = (IConditionCompare)References.Require(type);
                    compare = new CompareInfo(compareInst);
                    compares.Add(instance, compare);
                    return compare;
                }
            }
            return default;
        }

        internal IConditionHelper GetOrNewHelper(int type, int instance = ConditionHelperSetting.DEFAULT_INSTANCE)
        {
            if (m_Helpers.TryGetValue(type, out Dictionary<int, IConditionHelper> helpers))
            {
                if (helpers.TryGetValue(instance, out IConditionHelper helper))
                    return helper;

                if (m_HelpersType.TryGetValue(type, out Type helperType))
                {
                    helper = (IConditionHelper)References.Require(helperType);
                    helpers.Add(instance, helper);
                    return helper;
                }
            }
            return default;
        }

        /// <summary>
        /// 获取条件组句柄
        /// </summary>
        /// <param name="name">组名称</param>
        /// <returns>条件组句柄</returns>
        public IConditionGroupHandle Get(string name)
        {
            if (m_Groups.TryGetValue(name, out ConditionGroupHandle group))
                return group;
            return default;
        }

        /// <summary>
        /// 注册条件实例 查看<see cref="ConditionSetting"/>具体参数
        /// </summary>
        /// <param name="setting">条件配置</param>
        /// <returns>条件组句柄</returns>
        public IConditionGroupHandle Register(ConditionSetting setting)
        {
            if (m_Groups.TryGetValue(setting.Name, out ConditionGroupHandle group))
                return group;
            Log.Debug("Condition", $"Register {setting.Name} : {setting.Data}");
            group = new ConditionGroupHandle(setting, InnerGroupCompleteHandler);
            if (!group.IsDisposed)
            {
                m_Groups.Add(setting.Name, group);
                m_GroupList.Add(group);
            }
            return group;
        }

        /// <summary>
        /// 取消条件注册
        /// </summary>
        /// <param name="name">条件名</param>
        public void UnRegister(string name)
        {
            if (m_Groups.TryGetValue(name, out ConditionGroupHandle handle))
            {
                Log.Debug("Condition", $"UnRegister {name}");
                m_Groups.Remove(handle.Name);
                m_GroupList.Remove(handle);
                handle.Dispose();
            }
            else
            {
                Log.Debug("Condition", $"UnRegister {name}, but has not exist.");
            }
        }

        /// <summary>
        /// 取消条件注册
        /// </summary>
        /// <param name="name">条件组句柄</param>
        public void UnRegister(IConditionGroupHandle handle)
        {
            UnRegister(handle.Name);
        }

        private void InnerGroupCompleteHandler(IConditionGroupHandle group)
        {
            Log.Debug("Condition", $"{group.Name} has complete => {group.Setting.Data}");
            ConditionSetting setting = group.Setting;
            ConditionGroupHandle realGroup = (ConditionGroupHandle)group;
            if (setting.AutoRemove)
            {
                if (m_Groups.ContainsKey(group.Name))
                {
                    m_Groups.Remove(group.Name);
                    m_GroupList.Remove(realGroup);
                }
                realGroup.Dispose();
            }
        }

        private void InnerConditionTouchHandler(XEvent e)
        {
            ConditionEvent evt = (ConditionEvent)e;
            InnerTriggerCompare(evt.Target, evt.Param);
            for (int i = m_GroupList.Count - 1; i >= 0; i--)
            {
                if (i >= m_GroupList.Count)
                    continue;
                ConditionGroupHandle group = m_GroupList[i];
                if (!group.Complete)
                    group.InnerTrigger(evt.Target, evt.Param);
            }
        }

        private void InnerSpecificCondition(XEvent e)
        {
            SpecificConditionEvent evt = (SpecificConditionEvent)e;
            IConditionHandle handle = evt.Handle;
            IConditionGroupHandle group = handle.Group;

            if (m_Groups.TryGetValue(group.Name, out ConditionGroupHandle realGroup))
            {
                InnerTriggerCompare(handle.Target, evt.Param);
                if (!group.Complete)
                    realGroup.InnerTrigger(handle, evt.Param);
            }
            else
            {
                Log.Error("Condition", $"Module do not has {group.Name}");
            }
        }

        private void InnerConditionGroupTouchHandler(XEvent e)
        {
            ConditionGroupEvent evt = (ConditionGroupEvent)e;
            InnerTriggerGroup(evt.Handle.Name, evt.Target, evt.Param);
        }

        private void InnerTriggerGroup(string name, int target, object param)
        {
            if (m_Groups.TryGetValue(name, out ConditionGroupHandle group))
            {
                InnerTriggerCompare(target, param);
                if (!group.Complete)
                    group.InnerTrigger(target, param);
            }
            else
            {
                Log.Error("Condition", $"Module do not has {name}");
            }
        }

        private void InnerTriggerCompare(int target, object param)
        {
            if (m_Compares.TryGetValue(target, out Dictionary<int, CompareInfo> compares))
            {
                foreach (var item in compares)
                {
                    item.Value.OnEventTrigger(param);
                }
            }
        }
    }
}
