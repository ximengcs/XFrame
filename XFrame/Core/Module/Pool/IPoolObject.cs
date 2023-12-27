
namespace XFrame.Modules.Pools
{
    /// <summary>
    /// 对象池对象
    /// </summary>
    public interface IPoolObject
    {
        int PoolKey { get; }

        string MarkName { get; set; }

        IPool InPool { get; }
    }
}
