
namespace XFrame.Modules
{
    public partial class EntityData
    {
        public int Id { get; }
        public int TypeId { get; }

        public EntityData(int id, int type)
        {
            Id = id;
            TypeId = type;
        }
    }
}
