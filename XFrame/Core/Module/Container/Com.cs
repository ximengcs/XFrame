
namespace XFrame.Modules.Containers
{
    /// <summary>
    /// 组件基类
    /// 数据为组件所有
    /// </summary>
    public abstract class Com : Container, ICom
    {
        private bool m_Active;

        /// <summary>
        /// 组件激活状态
        /// </summary>
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

        /// <summary>
        /// 组件所属容器
        /// </summary>
        protected IContainer Owner => ((ICom)this).Owner;

        /// <summary>
        /// 激活生命周期
        /// </summary>
        protected virtual void OnActive() { }

        /// <summary>
        /// 失活生命周期
        /// </summary>
        protected virtual void OnInactive() { }
    }
}
