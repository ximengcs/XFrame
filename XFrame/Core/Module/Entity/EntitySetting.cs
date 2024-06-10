
using System;
using XFrame.Modules.Containers;

namespace XFrame.Modules.Entities
{
    public struct EntitySetting : IContainerSetting
    {
        private Type m_Type;

        public IContainer Master { get; set; }
        public bool ModuleUpdate { get; set; }
        public Type Type
        {
            get
            {
                if (m_Type == null)
                    return typeof(Container);
                else
                    return m_Type;
            }
        }
        public OnDataProviderReady DataProvider { get; set; }

        public EntitySetting(Type type)
        {
            Master = null;
            m_Type = type;
            ModuleUpdate = true;
            DataProvider = null;
        }

        public EntitySetting(Type type, IContainer master)
        {
            Master = master;
            m_Type = type;
            ModuleUpdate = true;
            DataProvider = null;
        }
    }
}
