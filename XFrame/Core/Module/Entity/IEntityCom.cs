using XFrame.Modules.Containers;

namespace XFrame.Modules.Entities
{
    public interface IEntityCom : IEntity, ICom
    {
        internal void OnInit(int id, IEntity owner, OnEntityComReady onReady);
    }
}
