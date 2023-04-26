
namespace XFrame.Modules.Containers
{
    /// <summary>
    /// 组件基类
    /// 数据为组件所有
    /// </summary>
    public abstract class Com : Container, ICom
    {
        private IContainer m_Owner;
        private bool m_Active;

        public bool Active
        {
            get { return m_Active; }
            set
            {
                if (m_Active != value)
                {
                    m_Active = value;
                    if (m_Active)
                        OnActive();
                    else
                        OnInactive();
                }
            }
        }

        public IContainer Owner => m_Owner;

        void ICom.OnInit(int id, IContainer owner, OnComReady onReady)
        {
            m_Owner = owner;
            IContainer thisContainer = this;
            thisContainer.OnInit(id, m_Owner.Master, (c) =>
            {
                onReady?.Invoke(this);
                Active = true;
            });
        }

        protected virtual void OnActive() { }
        protected virtual void OnInactive() { }
    }
}
