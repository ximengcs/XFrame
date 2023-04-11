using XFrame.Collections;
using XFrame.Core;

namespace XFrame.Modules.Containers
{
    public interface ICom : IXItem, IContainer
    {
        bool Active { get; set; }
        IDataProvider ShareData { get; }
        internal void OnInit(IContainer container, int id, object owner);
        internal void OnAwake();
        internal void OnUpdate();
        internal void OnDestroy();
    }
}
