using XFrame.Modules.Event;
using XFrame.Modules.Containers;

namespace XFrame.Modules.Entities
{
    /// <summary>
    /// 共享组件实体
    /// </summary>
    public class EntityShareCom : ShareCom, IEntityCom
    {
        private IEventSystem m_EventSys;

        public IEventSystem Event => m_EventSys;

        void IEntityCom.OnInit(int id, IEntity owner, OnEntityComReady onReady)
        {
            ICom thisCom = this;
            IEntity thisEntity = this;
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
