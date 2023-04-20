using XFrame.Modules.Containers;

namespace XFrame.Modules.Entities
{
    /// <summary>
    /// 实体组件
    /// </summary>
    public interface IEntityCom : IEntity, ICom
    {
        /// <summary>
        /// 初始化生命周期
        /// </summary>
        /// <param name="id">组件Id</param>
        /// <param name="owner">组件所属实体</param>
        /// <param name="onReady">组件就绪事件</param>
        void OnInit(int id, IEntity owner, OnEntityComReady onReady);
    }
}
