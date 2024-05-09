using System;
using XFrame.Modules.Containers;
using XFrame.Modules.ID;

namespace XFrame.Core.Module.Container
{
    internal class DefaultContainerHelper : IContainerHelper
    {
        private XDomain m_Domain;

        public DefaultContainerHelper(XDomain domain)
        {
            m_Domain = domain;
        }

        public int NextId(Type type)
        {
            return m_Domain.GetModule<IIdModule>().Next();
        }
    }
}
