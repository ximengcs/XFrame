using XFrame.Core;
using XFrame.Modules;

namespace XFrame.Collections
{
    public abstract class XItem : IXItem
    {
        public int Id { get; private set; }

        public void OnCreate()
        {
            Id = IDGenerator.Inst.Next();
        }

        public void OnRelease()
        {
            Id = default;
        }

        public void OnDestroyFrom()
        {
            Id = default;
        }
    }
}
