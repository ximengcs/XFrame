
namespace XFrame.Modules.Containers
{
    /// <summary>
    /// 组件基类
    /// 数据为组件所有
    /// </summary>
    public abstract class Com : Container, ICom, ICanSetOwner
    {
        private bool m_Active;
        private IContainer m_Owner;

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

        protected virtual void OnActive() { }
        protected virtual void OnInactive() { }

        void ICanSetOwner.SetOwner(IContainer owner)
        {
            m_Owner = owner;
        }
    }
}
