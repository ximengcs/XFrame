using System;
using System.Collections.Generic;
using System.Xml.Linq;
using XFrame.Core;
using XFrame.Modules.Archives;
using XFrame.Modules.Pools;

namespace XFrame.Modules.Conditions
{
    /// <summary>
    /// 条件组句柄，一个实例的条件控制句柄，一个实例可以有多个条件。
    /// <para>
    /// </para>
    /// </summary>
    internal class ConditionGroupHandle : IConditionGroupHandle
    {
        private bool m_Complete;
        private IDataProvider m_Data;
        private JsonArchive m_Archive;
        private IConditionHelper m_Helper;
        private ConditionSetting m_Setting;
        private List<IConditionHandle> m_AllInfos;
        private Action<IConditionGroupHandle> m_CompleteEvent;
        private Dictionary<int, List<IConditionHandle>> m_NotInfos;

        /// <summary>
        /// 条件组名称
        /// </summary>
        public string Name => m_Setting.Name;

        /// <summary>
        /// 条件是否完成
        /// </summary>
        public bool Complete => m_Complete;

        /// <summary>
        /// 条件配置
        /// </summary>
        public ConditionSetting Setting => m_Setting;

        /// <summary>
        /// 组内所有的条件
        /// </summary>
        public List<IConditionHandle> AllInfo => m_AllInfos;

        /// <summary>
        /// 组内还未达成的条件
        /// </summary>
        public Dictionary<int, List<IConditionHandle>> NotInfo => m_NotInfos;

        internal ConditionGroupHandle(ConditionSetting setting, Action<IConditionGroupHandle> completeCallback = null)
        {
            m_Setting = setting;
            m_Complete = false;
            m_CompleteEvent = completeCallback;
            m_AllInfos = new List<IConditionHandle>();
            m_NotInfos = new Dictionary<int, List<IConditionHandle>>();

            ConditionHelperSetting helperSetting = setting.HelperSetting;
            m_Helper = ConditionModule.Inst.GetOrNewHelper(setting.UseGroupHelper, helperSetting.UseInstance);
            if (helperSetting.UsePersistData)
            {
                InnerEnsureArchive();
                m_Data = m_Archive;
            }
            else
            {
                m_Data = new DataProvider();
            }

            var list = setting.Data.Parser.Value;
            foreach (var node in list)
            {
                ConditionHandle handle = new ConditionHandle(this, node.Value);
                int target = handle.Target;
                ConditionHelperSetting conditionSetting = setting.GetConditionHelperSettting(target);
                IConditionCompare compare = ConditionModule.Inst.GetOrNewCompare(target, conditionSetting.UseInstance);
                IDataProvider dataProvider;
                if (conditionSetting.UsePersistData)
                {
                    InnerEnsureArchive();
                    dataProvider = m_Archive.SpwanDataProvider($"condition_group_{m_Setting.Name}_{target}_{conditionSetting.UseInstance}");
                }
                else
                {
                    dataProvider = new DataProvider();
                }

                handle.OnInit(conditionSetting, compare, dataProvider);
                m_AllInfos.Add(handle);
                if (!handle.InnerCheckComplete())
                {
                    if (!m_NotInfos.TryGetValue(handle.Target, out List<IConditionHandle> conds))
                    {
                        conds = new List<IConditionHandle>();
                        m_NotInfos.Add(handle.Target, conds);
                    }
                    conds.Add(handle);
                }
            }

            if (m_Helper != null && m_Helper.CheckFinish(this))
            {
                m_Complete = true;
                m_CompleteEvent?.Invoke(this);
                m_CompleteEvent = null;
            }
            else
            {
                InnerCheckComplete();
            }
        }

        private void InnerEnsureArchive()
        {
            if (m_Archive != null)
                return;
            m_Archive = ArchiveModule.Inst.GetOrNew<JsonArchive>($"condition_group_{m_Setting.Name}_{m_Setting.HelperSetting.UseInstance}");
        }

        internal void InnerTrigger(IConditionHandle handle, object param)
        {
            if (m_NotInfos.TryGetValue(handle.Target, out List<IConditionHandle> handles))
            {
                int index = handles.IndexOf(handle);
                if (index != -1)
                {
                    ConditionHandle realHandle = (ConditionHandle)handle;
                    if (realHandle.InnerCheckComplete(param))
                    {
                        realHandle.MarkComplete();
                        handles.RemoveAt(index);
                    }
                }

                if (handles.Count == 0)
                    m_NotInfos.Remove(handle.Target);
                InnerCheckComplete();
            }
        }

        internal void InnerTrigger(int target, object param)
        {
            if (m_NotInfos.TryGetValue(target, out List<IConditionHandle> handles))
            {
                for (int i = handles.Count - 1; i >= 0; i--)
                {
                    ConditionHandle handle = (ConditionHandle)handles[i];
                    if (handle.InnerCheckComplete(param))
                    {
                        handle.MarkComplete();
                        handles.RemoveAt(i);
                    }
                }
                if (handles.Count == 0)
                    m_NotInfos.Remove(target);
                InnerCheckComplete();
            }
        }

        private void InnerCheckComplete()
        {
            if (m_NotInfos.Count == 0)
            {
                m_Complete = true;
                m_Helper?.MarkFinish(this);
                m_CompleteEvent?.Invoke(this);
                m_CompleteEvent = null;
            }
        }

        /// <summary>
        /// 释放条件实例
        /// </summary>
        internal void Dispose()
        {
            if (m_Setting.HelperSetting.IsUseInstance)
                References.Release(m_Helper);

            foreach (IConditionHandle h in m_AllInfos)
            {
                ConditionHandle handle = (ConditionHandle)h;
                handle.Dispose();
            }

            if (m_Data is JsonArchive archvie)
                archvie.Delete();

            m_Data = null;
            m_AllInfos = null;
            m_NotInfos = null;
        }

        /// <summary>
        /// 注册完成回调
        /// </summary>
        /// <param name="callback">回调</param>
        public void OnComplete(Action<IConditionGroupHandle> callback)
        {
            if (m_Complete)
                callback?.Invoke(this);
            else
                m_CompleteEvent += callback;
        }

        public void SetData<T>(T value)
        {
            m_Data.SetData(value);
        }

        public T GetData<T>()
        {
            return m_Data.GetData<T>();
        }

        public void SetData<T>(string name, T value)
        {
            m_Data.SetData<T>(name, value);
        }

        public T GetData<T>(string name)
        {
            return m_Data.GetData<T>(name);
        }

        public void ClearData()
        {
            m_Data.ClearData();
        }
    }
}
