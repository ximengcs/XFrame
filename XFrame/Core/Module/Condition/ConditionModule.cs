using XFrame.Core;
using XFrame.Modules.Event;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;
using XFrame.Modules.XType;
using System;
using XFrame.Collections;

namespace XFrame.Modules.Conditions
{
    /// <summary>
    /// 条件监听模块
    /// </summary>
    [XModule]
    public class ConditionModule : SingletonModule<ConditionModule>
    {
        private IEventSystem m_Event;
        private Dictionary<int, IConditionHelper> m_Helpers;
        private Dictionary<int, IConditionCompare> m_Compares;
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
            m_Compares = new Dictionary<int, IConditionCompare>();
            m_Groups = new Dictionary<string, ConditionGroupHandle>();
            m_Helpers = new Dictionary<int, IConditionHelper>();

            TypeSystem typeSys = TypeModule.Inst.GetOrNew<IConditionCompare>();
            foreach (Type type in typeSys)
            {
                IConditionCompare compare = (IConditionCompare)TypeModule.Inst.CreateInstance(type);
                m_Compares.Add(compare.Target, compare);
            }

            typeSys = TypeModule.Inst.GetOrNew<IConditionHelper>();
            foreach (Type type in typeSys)
            {
                IConditionHelper helper = (IConditionHelper)TypeModule.Inst.CreateInstance(type);
                m_Helpers.Add(helper.Type, helper);
            }
        }

        public IConditionCompare GetCompare(int target)
        {
            if (m_Compares.TryGetValue(target, out IConditionCompare compare))
                return compare;
            return default;
        }

        public Type GetCompareType(int target)
        {
            IConditionCompare compare = GetCompare(target);
            if (compare == null)
                return compare.GetType();
            else
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
            Log.Debug("Condition", $"Register {setting.Name} : {setting.Condition}");
            m_Helpers.TryGetValue(setting.UseGroupHelper, out IConditionHelper helper);
            group = new ConditionGroupHandle(setting, helper, InnerGroupCompleteHandler);
            m_Groups.Add(setting.Name, group);
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
                Log.Debug("Condition", $"UnRegister {name} : {handle.Setting.Condition}");
                m_Groups.Remove(handle.Name);
                handle.Dispose();
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
            Log.Debug("Condition", $"{group.Name} has complete => {group.Setting.Condition}");
            ConditionSetting setting = group.Setting;
            if (setting.AutoRemove)
                m_Groups.Remove(group.Name);
            ConditionGroupHandle g = (ConditionGroupHandle)group;
            g.Dispose();
        }

        private void InnerConditionTouchHandler(XEvent e)
        {
            ConditionEvent evt = (ConditionEvent)e;
            InnerTriggerCompare(evt.Target, evt.Param);
            foreach (var item in m_Groups)
                item.Value.InnerTrigger(evt.Target, evt.Param);
        }

        private void InnerSpecificCondition(XEvent e)
        {
            SpecificConditionEvent evt = (SpecificConditionEvent)e;
            IConditionHandle handle = evt.Handle;
            IConditionGroupHandle group = handle.Group;
            InnerTriggerGroup(group.Name, handle.Target, evt.Param);
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
                group.InnerTrigger(target, param);
            }
            else
            {
                Log.Error("Condition", $"Module do not has {name}");
            }
        }

        private void InnerTriggerCompare(int target, object param)
        {
            if (m_Compares.TryGetValue(target, out IConditionCompare compare))
                compare.OnEventTrigger(param);
        }
    }
}
