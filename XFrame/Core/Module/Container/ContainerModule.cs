using XFrame.Core;
using XFrame.Collections;

namespace XFrame.Modules.Containers
{
    [XModule]
    public partial class ContainerModule : SingletonModule<ContainerModule>
    {
        private XCollection<Container> m_Containers;

        public IContainer New(object owner = null)
        {
            Container container = new Container();
            container.OnInit(owner);
            m_Containers.Add(container);
            return container;
        }

        public void Remove(IContainer container)
        {
            Container cont = (Container)container;
            if (cont != null)
            {
                cont.OnDestroy();
                m_Containers.Remove(cont);
            }
        }

        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_Containers = new XCollection<Container>();
        }

        protected override void OnUpdate(float escapeTime)
        {
            base.OnUpdate(escapeTime);
            foreach (Container cont in m_Containers)
                cont.OnUpdate();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            foreach (Container cont in m_Containers)
                cont.OnDestroy();
            m_Containers.Clear();
        }
    }
}
