using XFrame.Modules.Event;
using XFrame.Modules.Containers;

namespace XFrame.Modules.Entities
{
    /// <summary>
    /// 共享组件实体
    /// </summary>
    public class EntityShareCom : ShareCom, IEntityCom
    {
        private IEntity m_Parent;

        public IEventSystem Event => m_Parent.Event;
        public IEntity Parent => m_Parent;

        void IEntityCom.OnInit(int id, IEntity owner, OnEntityComReady onReady)
        {
            ICom thisCom = this;
            IEntity thisEntity = this;
            m_Parent = owner;
            thisEntity.OnInit(id, owner, null);
            thisCom.OnInit(id, owner, (c) => onReady?.Invoke(this));
        }

        void IEntity.OnInit(int id, IEntity parent, OnEntityReady onData)
        {

        }
    }
}
