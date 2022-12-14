
namespace XFrame.Modules
{
    /// <summary>
    /// 实体数据
    /// </summary>
    public partial class EntityData
    {
        /// <summary>
        /// 实体Id
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// 实体类型Id，实体模块创建实体时会通过此Id获取到对应的实体类，并创建出实体实例
        /// </summary>
        public int TypeId { get; }

        /// <summary>
        /// 构建实体数据
        /// </summary>
        /// <param name="id">实体Id</param>
        /// <param name="type">实体类型</param>
        public EntityData(int id, int type)
        {
            Id = id;
            TypeId = type;
        }
    }
}
