using XFrame.Modules.Event;
using XFrame.Modules.Containers;

namespace XFrame.Modules.Entities
{
    /// <summary>
    /// 实体组件
    /// </summary>
    public abstract class EntityCom : Com, IEntityCom
    {
        private IEventSystem m_EventSys;
        private IEntity m_Parent;

        public IEventSystem Event => m_EventSys;
        public IEntity Parent => m_Parent;

        void IEntityCom.OnInit(int id, IEntity owner, OnEntityComReady onReady)
        {
            ICom thisCom = this;
            IEntity thisEntity = this;
            m_Parent = owner;
            thisEntity.OnInit(id, owner, (e) =>
            {
                thisCom.OnInit(id, owner, (c) => onReady?.Invoke(this));
            });
        }

        void IEntity.OnInit(int id, IEntity parent, OnEntityReady onData)
        {
            m_EventSys = EventModule.Inst.NewSys();
        }
    }
}
