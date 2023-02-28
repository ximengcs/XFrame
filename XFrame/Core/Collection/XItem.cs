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

        public XItem()
        {
            Id = IdModule.Inst.Next();
        }
    }
}
