using XFrame.Core;
using XFrame.Modules;

namespace XFrame.Collections
{
    public abstract class XItem : IXItem
    {
        public int Id { get; private set; }

        public void OnCreate(IPool from)
        {
            Id = IDGenerator.Inst.Next();
        }

        public void OnRelease(IPool from)
        {
            Id = default;
        }

        public void OnDestroy(IPool from)
        {
            Id = default;
        }
    }
}
