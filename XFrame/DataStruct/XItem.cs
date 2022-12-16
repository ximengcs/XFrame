using XFrame.Modules.ID;
using XFrame.Modules.Pools;

namespace XFrame.Collections
{
    /// <summary>
    /// XCollection集合元素抽象类，Id自动通过IDModule获取
    /// </summary>
    public abstract class XItem : IXItem
    {
        public int Id { get; private set; }

        void IPoolObject.OnCreate()
        {
            Id = IdModule.Inst.Next();
        }

        void IPoolObject.OnRelease()
        {
            Id = default;
        }

        void IPoolObject.OnDestroyForever()
        {
            Id = default;
        }
    }
}
