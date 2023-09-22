using System;
using System.Collections.Generic;
using XFrame.Core;

namespace XFrame.Modules.Conditions
{
    /// <summary>
    /// 条件辅助器配置
    /// </summary>
    public struct ConditionHelperSetting
    {
        /// <summary>
        /// 全局辅助器实例Id
        /// </summary>
        public const int DEFAULT_INSTANCE = 0;

        /// <summary>
        /// 使用的辅助器实例
        /// </summary>
        public int UseInstance { get; }

        /// <summary>
        /// 是否使用持久化的数据
        /// </summary>
        public bool UsePersistData { get; }

        /// <summary>
        /// 是否使用非全局辅助器
        /// </summary>
        public bool IsUseInstance => UseInstance != DEFAULT_INSTANCE;

        private ConditionHelperSetting(int useInstance, bool usePersistData)
        {
            UseInstance = useInstance;
            UsePersistData = usePersistData;
        }

        public static ConditionHelperSetting Create(int useInstance = DEFAULT_INSTANCE, bool usePersistData = false)
        {
            return new ConditionHelperSetting(useInstance, usePersistData);
        }

        public static ConditionHelperSetting Create(bool usePersistData)
        {
            return new ConditionHelperSetting(DEFAULT_INSTANCE, usePersistData);
        }

        public override bool Equals(object obj)
        {
            ConditionHelperSetting other = (ConditionHelperSetting)obj;
            return UseInstance == other.UseInstance && UsePersistData == other.UsePersistData;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(UseInstance, UsePersistData);
        }

        public static bool operator ==(ConditionHelperSetting a, ConditionHelperSetting b)
        {
            return a.UseInstance == b.UseInstance && a.UsePersistData == b.UsePersistData;
        }

        public static bool operator !=(ConditionHelperSetting a, ConditionHelperSetting b)
        {
            return a.UseInstance != b.UseInstance || a.UsePersistData != b.UsePersistData;
        }
    }

    /// <summary>
    /// 条件配置
    /// </summary>
    public struct ConditionSetting
    {
        private Dictionary<int, ConditionHelperSetting> m_ConditionHelperInstance;

        /// <summary>
        /// 条件名称
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 条件组使用的辅助器 <see cref="IConditionHelper"/> 类型，与<see cref="IConditionHelper.Type"/>相匹配
        /// </summary>
        public int UseGroupHelper { get; }

        /// <summary>
        /// 条件组辅助器配置
        /// </summary>
        public ConditionHelperSetting HelperSetting { get; }

        /// <summary>
        /// 在条件组完成时自动清理并从模块移除
        /// 当为 true 时，在条件达成时自动从<see cref="IConditionModule"/>模块中移除此条件，会调用<see cref="ConditionGroupHandle.Dispose"/>清理监听，
        /// 此时通过<see cref="IConditionModule.Get(string)"/>将获取不到条件实例。
        /// 当为 false 时，在条件达成时不会自动从<see cref="IConditionModule"/>模块中移除此条件，但仍会调用<see cref="ConditionGroupHandle.Dispose"/>清理。
        /// 置为false时，需要调用者在不使用条件句柄后手动调用<see cref="IConditionModule.UnRegister"/>移除条件的句柄，
        /// 否则将一直存在于条件模块<see cref="IConditionModule"/>中
        /// </summary>
        public bool AutoRemove { get; }

        /// <summary>
        /// 原始条件数据
        /// 例：
        /// [条件1],[条件2]...
        /// [条件1] : {条件类型|目标参数} 其中条件类型<see cref="int"/>即 <see cref="ConditionEvent.Target"/> 需要触发的类型
        /// 目标参数类型为 <see cref="UniversalParser"/> ，可二次分析
        /// </summary>
        public ConditionData Data;

        /// <summary>
        /// 设置条件项辅助器配置
        /// </summary>
        /// <param name="target">条件项目标类型</param>
        /// <param name="settting">辅助器配置</param>
        public void SetConditionHelperSetting(int target, ConditionHelperSetting settting)
        {
            if (m_ConditionHelperInstance == null)
                m_ConditionHelperInstance = new Dictionary<int, ConditionHelperSetting>();
            if (m_ConditionHelperInstance.ContainsKey(target))
                m_ConditionHelperInstance[target] = settting;
            else
                m_ConditionHelperInstance.Add(target, settting);
        }

        /// <summary>
        /// 获取条件项辅助器配置
        /// </summary>
        /// <param name="target">条件项目标类型</param>
        /// <returns>辅助器配置</returns>
        public ConditionHelperSetting GetConditionHelperSettting(int target)
        {
            if (m_ConditionHelperInstance == null)
                return ConditionHelperSetting.Create();
            if (m_ConditionHelperInstance.TryGetValue(target, out ConditionHelperSetting settting))
                return settting;
            return ConditionHelperSetting.Create();
        }

        /// <summary>
        /// 构造条件配置，默认<see cref="AutoRemove"/> 为 <see cref="true"/>, <see cref="UseGroupHelper"/> 为 0
        /// </summary>
        /// <param name="name">条件名称</param>
        /// <param name="data">原始条件配置</param>
        public ConditionSetting(string name, ConditionData data, ConditionHelperSetting helper = default)
        {
            Name = name;
            Data = data;
            AutoRemove = true;
            UseGroupHelper = 0;
            HelperSetting = helper;
            m_ConditionHelperInstance = null;
        }

        /// <summary>
        /// 构造条件配置，默认<see cref="AutoRemove"/> 为 <see cref="true"/>
        /// </summary>
        /// <param name="name">条件名称</param>
        /// <param name="data">原始条件配置</param>
        /// <param name="useHelper">使用辅助器</param>
        public ConditionSetting(string name, ConditionData data, int useHelper, ConditionHelperSetting helper = default)
        {
            Name = name;
            Data = data;
            AutoRemove = true;
            UseGroupHelper = useHelper;
            HelperSetting = helper == default ? ConditionHelperSetting.Create() : helper;
            m_ConditionHelperInstance = null;
        }

        /// <summary>
        /// 构造条件配置，默认<see cref="UseGroupHelper"/> 为 0
        /// </summary>
        /// <param name="name">条件名称</param>
        /// <param name="data">原始条件配置</param>
        /// <param name="autoRemove">是否自动移除</param>
        public ConditionSetting(string name, ConditionData data, bool autoRemove, ConditionHelperSetting helper = default)
        {
            Name = name;
            Data = data;
            AutoRemove = autoRemove;
            UseGroupHelper = 0;
            HelperSetting = helper == default ? ConditionHelperSetting.Create() : helper;
            m_ConditionHelperInstance = null;
        }

        /// <summary>
        /// 构造条件配置
        /// </summary>
        /// <param name="name">条件名称</param>
        /// <param name="data">原始条件配置</param>
        /// <param name="autoRemove">是否自动移除</param>
        /// <param name="useHelper">使用辅助器</param>
        public ConditionSetting(string name, ConditionData data, bool autoRemove, int useHelper, ConditionHelperSetting helper = default)
        {
            Name = name;
            Data = data;
            AutoRemove = autoRemove;
            UseGroupHelper = useHelper;
            HelperSetting = helper == default ? ConditionHelperSetting.Create() : helper;
            m_ConditionHelperInstance = null;
        }
    }
}
