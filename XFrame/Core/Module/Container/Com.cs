
namespace XFrame.Modules.Containers
{
    /// <summary>
    /// 组件基类
    /// 数据为组件所有
    /// </summary>
    public abstract class Com : Container, ICom
    {
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

        IContainer ICom.Owner { get; set; }
        protected IContainer Owner => Owner;
        protected virtual void OnActive() { }
        protected virtual void OnInactive() { }
    }
}
