
namespace XFrame.Modules.Pools
{
    public class CommonPoolObject<T> : PoolObjectBase, IPoolObject
    {
        public T Target { get; set; }

        public bool Valid => Target != null;
    }
}
