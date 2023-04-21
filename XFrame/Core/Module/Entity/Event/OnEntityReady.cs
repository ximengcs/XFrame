
namespace XFrame.Modules.Entities
{
    public delegate void OnEntityReady(IEntity entity);

    public delegate void OnEntityReady<T>(T entity) where T : IEntity;
}