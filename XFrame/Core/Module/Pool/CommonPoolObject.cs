
namespace XFrame.Modules.Pools
{
    public class CommonPoolObject<T> : PoolObjectBase
    {
        public T Target { get; set; }

        public bool Valid => Target != null;
    }
}
