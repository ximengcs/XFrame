using XFrame.Core;
using XFrame.Modules.Event;

namespace XFrame.Modules.Entities
{
    /// <summary>
    /// 实体模块
    /// 只有根实体才会接受实体模块的更新生命周期
    /// </summary>
    public interface IEntityModule : IModule
    {
        IEventSystem Event { get; }

        void SetHelper(IEntityHelper helper);

        /// <summary>
        /// 根据Id获取实体
        /// </summary>
        /// <param name="entityId">实体Id</param>
        /// <returns>实体</returns>
        IEntity Get(int entityId);

        IEntity Create(int entityId, EntitySetting setting);

        IEntity Create(EntitySetting setting);

        /// <summary>
        /// 销毁一个实体
        /// </summary>
        /// <param name="entity">需要销毁的实体</param>
        void Destroy(IEntity entity);

        void Destroy(int entityId);
    }
}
